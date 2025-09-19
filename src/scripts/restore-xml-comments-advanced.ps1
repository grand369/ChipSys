$currRoot = "E:\AI_Root\ChipPlatformSys\Admin.Core\src\modules\admin"
$bakRoot  = "E:\AI_Root\chip_platform_sys\Admin.Core\src\modules\admin"

if (-not (Test-Path $currRoot)) { Write-Host "当前目录不存在: $currRoot"; exit 1 }
if (-not (Test-Path $bakRoot))  { Write-Host "备份目录不存在: $bakRoot";  exit 1 }

$updated = 0; $skipped = 0; $missing = 0; $mappedOnly = 0

function Get-DocRanges {
  param([string[]]$lines)
  $ranges = @()
  $i = 0; $n = $lines.Count
  while ($i -lt $n) {
    if ($lines[$i].TrimStart().StartsWith('///')) {
      $start = $i
      while ($i -lt $n -and $lines[$i].TrimStart().StartsWith('///')) { $i++ }
      $end = $i - 1
      $ranges += [pscustomobject]@{ Start = $start; End = $end }
    } else { $i++ }
  }
  return ,$ranges
}

# 常见乱码 -> 中文 映射，仅作用于注释行
$map = @{
  'ʹ�ö�����' = '使用多语言'
  'Ĭ����'     = '默认区域'
  'Ĭ���⻧'   = '默认租户'
  '����'       = '是否启用'
  '��������'   = '扫描的程序集'
  'Զ�̹��̵�������' = '远程调用的配置项'
  'HttpԶ������' = 'Http 远程调用'
  'GrpcԶ������' = 'Grpc 远程调用'
  '��ַ'       = '地址'
  '��ַ�б�'   = '地址列表'
  '����OSSע��' = '未配置 OSS 时注册占位实现'
  '�����洢Ͱ' = '创建存储桶'
  '�鿴�洢ͰȨ��' = '查看存储桶权限'
  '���ô洢ͰȨ��' = '设置存储桶权限'
  '����Minio�洢ͰȨ��' = '设置 Minio 存储桶权限'
  '��Ӧ������' = '响应委托处理程序'
  '��Ӧ��֤������' = '响应认证处理程序'
  '������־����' = '操作日志处理'
  '���IP��ַ' = '解析 IP 地址归属地'
}

function Apply-MappingToComments {
  param([string[]]$lines)
  $changed = $false
  for ($i=0; $i -lt $lines.Count; $i++) {
    $t = $lines[$i].TrimStart()
    $isComment = $t.StartsWith('///') -or $t.StartsWith('//') -or $t.StartsWith('/*') -or $t.StartsWith('*')
    if ($isComment) {
      foreach ($k in $map.Keys) {
        if ($lines[$i].Contains($k)) {
          $lines[$i] = $lines[$i].Replace($k, $map[$k])
          $changed = $true
        }
      }
    }
  }
  return ,@($changed, $lines)
}

Get-ChildItem -Path $currRoot -Recurse -Filter *.cs | ForEach-Object {
  $currFile = $_.FullName
  $relPath  = $currFile.Substring($currRoot.Length)
  $relPath  = $relPath.TrimStart([char[]]"\\/")

  # 构造候选备份路径（兼容 ChipSys.Admin.Core 与 ZhonTai.Admin.Core 差异）
  $candidates = @()
  $candidates += (Join-Path $bakRoot $relPath)
  if ($relPath -match 'ChipSys\.Admin\.Core') {
    $candidates += (Join-Path $bakRoot ($relPath -replace 'ChipSys\.Admin\.Core','ZhonTai.Admin.Core'))
  }
  if ($relPath -match 'ZhonTai\.Admin\.Core') {
    $candidates += (Join-Path $bakRoot ($relPath -replace 'ZhonTai\.Admin\.Core','ChipSys.Admin.Core'))
  }

  $bakFile = $null
  foreach ($cand in $candidates) {
    if (Test-Path $cand) { $bakFile = $cand; break }
  }

  $currLines = Get-Content -LiteralPath $currFile -Encoding UTF8

  if ($bakFile) {
    $bakLines  = Get-Content -LiteralPath $bakFile  -Encoding UTF8

    $currRanges = Get-DocRanges -lines $currLines
    $bakRanges  = Get-DocRanges -lines $bakLines

    $work = [System.Collections.Generic.List[string]]::new(); $work.AddRange($currLines)
    $didXmlReplace = $false

    if ($currRanges.Count -gt 0 -and $bakRanges.Count -gt 0) {
      $count = [Math]::Min($currRanges.Count, $bakRanges.Count)
      for ($k = $count - 1; $k -ge 0; $k--) {
        $cr = $currRanges[$k]; $br = $bakRanges[$k]
        $newBlock = $bakLines[$br.Start..$br.End]
        $work.RemoveRange($cr.Start, $cr.End - $cr.Start + 1)
        $work.InsertRange($cr.Start, $newBlock)
        $didXmlReplace = $true
      }
    }

    # 对普通注释应用映射
    $mappedResult = Apply-MappingToComments -lines $work
    $mappedChanged = $mappedResult[0]; $work = [System.Collections.Generic.List[string]]($mappedResult[1])

    if ($didXmlReplace -or $mappedChanged) {
      Set-Content -LiteralPath $currFile -Value $work -Encoding UTF8
      $updated++
    } else {
      $skipped++
    }
  }
  else {
    # 无备份文件，仅对注释应用映射
    $mappedResult = Apply-MappingToComments -lines $currLines
    $mappedChanged = $mappedResult[0]; $outLines = $mappedResult[1]
    if ($mappedChanged) {
      Set-Content -LiteralPath $currFile -Value $outLines -Encoding UTF8
      $mappedOnly++
    } else {
      $missing++
    }
  }
}

Write-Host "完成(高级方式)。更新(含映射): $updated，仅映射: $mappedOnly，跳过: $skipped，备份缺失: $missing"


