$currRoot = "E:\AI_Root\ChipPlatformSys\Admin.Core\src\modules\admin"
$bakRoot  = "E:\AI_Root\chip_platform_sys\Admin.Core\src\modules\admin"

if (-not (Test-Path $currRoot)) { Write-Host "当前目录不存在: $currRoot"; exit 1 }
if (-not (Test-Path $bakRoot))  { Write-Host "备份目录不存在: $bakRoot";  exit 1 }

$restored = 0; $skipped = 0; $missing = 0

function Find-BackupFile {
  param([string]$relPath)
  $candidates = @()
  $candidates += (Join-Path $bakRoot $relPath)
  if ($relPath -match 'ChipSys\.Admin\.Core') {
    $candidates += (Join-Path $bakRoot ($relPath -replace 'ChipSys\.Admin\.Core','ZhonTai.Admin.Core'))
  }
  if ($relPath -match 'ZhonTai\.Admin\.Core') {
    $candidates += (Join-Path $bakRoot ($relPath -replace 'ZhonTai\.Admin\.Core','ChipSys.Admin.Core'))
  }
  foreach ($cand in $candidates) { if (Test-Path $cand) { return $cand } }
  return $null
}

function Rewrite-Namespaces {
  param([string]$content)
  $out = $content
  $out = $out -replace 'namespace\s+ZhonTai\.Admin\.Core', 'namespace ChipSys.Admin.Core'
  $out = $out -replace 'using\s+ZhonTai\.Admin\.Core', 'using ChipSys.Admin.Core'
  $out = $out -replace 'ZhonTai\.Admin\.Core', 'ChipSys.Admin.Core'
  $out = $out -replace 'ZhonTai\.Admin', 'ChipSys.Admin'
  return $out
}

Get-ChildItem -Path $currRoot -Recurse -Filter *.cs | ForEach-Object {
  $currFile = $_.FullName
  $relPath  = $currFile.Substring($currRoot.Length)
  $relPath  = $relPath.TrimStart([char[]]"\\/")

  # 判定是否内容被清空/仅1-2行（XML头等特殊文件除外）
  $lines = Get-Content -LiteralPath $currFile -Encoding UTF8 -Raw
  $lineCount = ($lines -split "\r?\n").Count
  if ($lineCount -gt 2 -and ($lines.Trim().Length -gt 0)) { $skipped++; return }

  $bakFile = Find-BackupFile -relPath $relPath
  if (-not $bakFile) { $missing++; return }

  $bakContent = Get-Content -LiteralPath $bakFile -Encoding UTF8 -Raw
  if ([string]::IsNullOrWhiteSpace($bakContent)) { $missing++; return }

  # 命名空间重写
  $restoredContent = Rewrite-Namespaces -content $bakContent

  Set-Content -LiteralPath $currFile -Value $restoredContent -Encoding UTF8
  $restored++
}

Write-Host "完成(还原被清空文件)。已还原: $restored，跳过: $skipped，备份缺失: $missing"


