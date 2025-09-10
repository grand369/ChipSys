interface JwtPayload {
  [key: string]: any
}

/**
 * jwt token解密
 * @param {String} token jwt token字符�?
 */
export function parseJwtToken(token: string): JwtPayload | null {
  const base64Url = token?.split('.')[1]
  if (!base64Url) {
    return null
  }

  const base64 = base64Url.replace('-', '+').replace('_', '/')
  try {
    const payload = JSON.parse(window.atob(base64))
    return payload
  } catch {
    return null
  }
}

/**
 * 获得文件后缀�?
 * @param {String} filename 文件�?
 */
export const getFileExtension = (filename: string): string => {
  const index = filename.lastIndexOf('.')
  return index >= 0 ? filename.substring(index) : ''
}

export default {
  parseJwtToken,
  getFileExtension,
}
