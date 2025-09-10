import { cloneDeep, merge, isEmpty, isFunction } from 'lodash-es'

/**
* @description: 列表转树形列�?
* @example
listToTree(cloneDeep(list))

listToTree(cloneDeep(list), {
  idKey: 'id',
  parentIdKey: 'parentId',
  childrenKey: 'children',
  extraData: null
})
*/
export function listToTree(list: any = [], options = {}) {
  const { idKey, parentIdKey, childrenKey, extraData } = Object.assign(
    {
      idKey: 'id',
      parentIdKey: 'parentId',
      childrenKey: 'children',
      extraData: null as any,
    },
    options || {}
  )
  const idMap: { [key: string]: any } = {}
  list.forEach((item: any) => {
    idMap[item[idKey]] = cloneDeep(item)
    if (isFunction(extraData)) {
      extraData(idMap[item[idKey]])
    } else if (!isEmpty(extraData)) {
      merge(idMap[item[idKey]], extraData)
    }
    idMap[item[idKey]][childrenKey] = []
  })

  const tree = [] as any
  list.forEach((item: any) => {
    const parentId = item[parentIdKey]
    if (idMap[parentId]) {
      idMap[parentId][childrenKey].push(idMap[item[idKey]])
    } else {
      tree.push(idMap[item[idKey]])
    }
  })

  return tree
}

/**
* @description: 将树形列表转换为扁平化数据列�?
* @example
toFlatList(tree, (data) => { return data['children'] }, list)
*/
export function toFlatList(tree: any, getChilds: any, flatList: any = [], noChildren = true) {
  tree.forEach((item: any) => {
    flatList.push(item)
    const children = getChilds(item)
    if (children?.length > 0) {
      toFlatList(children, getChilds, flatList, noChildren)
    }
    if (noChildren) {
      delete item.children
    }
  })
}

/**
* @description: 树形列表转列表无子级
* @example
treeToList(cloneDeep(tree))

treeToList(cloneDeep(tree), (data) => { return data['children'] })
*/
export function treeToList(
  tree: any = [],
  getChilds = (data: any) => {
    return data['children']
  }
) {
  const list = [] as any
  // 空树
  if (!(tree?.length > 0)) {
    return list
  }

  toFlatList(tree, getChilds, list)

  return list
}

/**
* @description: 树形列表过滤父级或者子级数�?
* @example
filterTree(cloneDeep(tree), keyword)

filterTree(cloneDeep(tree), keyword, {
  children: 'children',
  filterWhere: (item: any, filterword: string) => {
    return item.name?.toLocaleLowerCase().indexOf(filterword) > -1
  },
})
*/
export function filterTree(tree: any = [], keyword: string, options = {}) {
  const { children, filterWhere } = Object.assign(
    {
      children: 'children',
      filterWhere: (item: any, word: string) => {
        return item.name?.toLocaleLowerCase().indexOf(word?.toLocaleLowerCase()) > -1
      },
    },
    options || {}
  )

  return tree.filter((item: any) => {
    if (filterWhere(item, keyword)) {
      return true
    }

    if (item[children]) {
      item[children] = filterTree(item[children], keyword, { children, filterWhere })
      return item[children].length > 0
    }

    return false
  })
}

/**
* @description: 列表过滤数据并返回所有父级数�?
* @example
filterList(cloneDeep(list), keyword)

filterList(cloneDeep(list), keyword, {
      parentWhere: (item: any, parent: any) => {
        return item.id === parent.parentId
      },
      parentEndWhere: (item: any) => {
        return item.parentId > 0
      },
      filterWhere: (item: any, word: string) => {
        return item.label?.toLocaleLowerCase().includes(word)
      },
})
*/
export function filterList(list: any[], keyword: string, options = {}) {
  const { parentWhere, parentEndWhere, filterWhere } = Object.assign(
    {
      parentWhere: (item: any, parent: any) => {
        return item.id === parent.parentId
      },
      parentEndWhere: (item: any) => {
        return item.parentId > 0
      },
      filterWhere: (item: any, word: string) => {
        return item.label?.toLocaleLowerCase().includes(word)
      },
    },
    options || {}
  )

  let dataList: any[] = []

  function searchParentList(parent: any, list: any[]): any[] {
    let parentList: any[] = []
    for (let i = 0; i < list.length; i++) {
      const item = list[i]
      if (parentWhere(item, parent)) {
        parentList.push(item)
        if (parentEndWhere(item)) {
          const parentNodesData = searchParentList(item, list)
          parentList.push(...parentNodesData)
        }
      }
    }
    return parentList
  }

  for (let i = 0; i < list.length; i++) {
    const item = list[i]
    if (filterWhere(item, keyword)) {
      dataList.push(item)
      const parentList = searchParentList(item, list)
      dataList.push(...parentList)
    }
  }

  const uniqueNodes: { [id: string | number]: any } = {}
  for (const item of dataList) {
    uniqueNodes[item.id] = item
  }

  dataList = Object.values(uniqueNodes)

  return dataList
}

/**
* @description: 树形列表转列表包含子�?
* @example
treeToListWithChildren(cloneDeep(tree))

treeToListWithChildren(cloneDeep(tree), (data) => { return data['children'] })
*/
export function treeToListWithChildren(
  tree = [],
  getChilds = (data: any) => {
    return data['children']
  }
) {
  const list = [] as any
  // 空树
  if (!(tree?.length > 0)) {
    return list
  }

  toFlatList(tree, getChilds, list, false)

  return list
}

/**
* @description: 获得自身所有父级列�?
* @example
getParents(cloneDeep(items), self)
getParents(treeToList(cloneDeep(items), self))

const parents = getParents(cloneDeep(items), self, (item, self) => {
	return item.id === self.parentId
})
*/
export function getParents(
  list = [],
  self: any,
  parentWhere = (item: any, self: any) => {
    return item.id === self.parentId
  },
  parents = []
) {
  // 空列�?
  if (!(list?.length > 0)) {
    return parents
  }

  if (!self) {
    return parents
  }

  const parent = list.find((item) => parentWhere && parentWhere(item, self))

  if (parent) {
    parents.unshift(parent)
    getParents(list, parent, parentWhere, parents)
  }

  return parents
}

/**
* @description: 获得自身所有父级列表包含自�?
* @example
getParentsAndSelf(cloneDeep(items), self)
getParentsAndSelf(treeToList(cloneDeep(items)), self)

const parents = getParentsAndSelf(cloneDeep(items), self, {
  selfWhere: (item, self) => {
    return item.id === self.id
  },
  parentWhere: (item, self) => {
    return item.id === self.parentId
  }
})
*/
export function getParentsAndSelf(list = [], self: any, options = {}) {
  const { selfWhere, parentWhere } = Object.assign(
    {
      selfWhere: (item: any, self: any) => {
        return item.id === self.id
      },
      parentWhere: (item: any, self: any) => {
        return item.id === self.parentId
      },
    },
    options || {}
  )

  const parents = getParents(list, self, parentWhere)
  const me = list.find((item: any) => selfWhere && selfWhere(item, self))
  if (me) {
    parents.unshift(me)
  }

  return parents
}

export default {
  toTree: listToTree,
  toList: treeToList,
  toListWithChildren: treeToListWithChildren,
  getParents,
  getParentsAndSelf,
}
