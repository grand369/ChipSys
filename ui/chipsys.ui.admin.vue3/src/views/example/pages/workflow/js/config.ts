// jsplumb 默认配置
export const jsplumbDefaults = {
  // 多个锚点 [源锚点，目标锚点]
  Anchors: [
    'Top',
    'TopCenter',
    'TopRight',
    'TopLeft',
    'Right',
    'RightMiddle',
    'Bottom',
    'BottomCenter',
    'BottomRight',
    'BottomLeft',
    'Left',
    'LeftMiddle',
  ],
  // 连线的容器id
  Container: 'workflow-right',
  // 设置链接线的形状，如直线或者曲线之类的。anchor可以去设置锚点的位置。可选�?<Bezier|Flowchart|StateMachine|Straight>"
  Connector: ['Bezier', { curviness: 100 }],
  // 节点是否可以用鼠标拖动使其断开，默认为true。即用鼠标链接上的连线，也可以使用鼠标拖动让其断开。设置成false，可以让其拖动也不会自动断开
  ConnectionsDetachable: false,
  // 删除线的时候节点不删除
  DeleteEndpointsOnDetach: false,
  //  每当添加或以其他方式创建 Endpoint 并且 jsPlumb 尚未给出任何明确�?Endpoint 定义时将使用
  Endpoint: ['Blank', { Overlays: '' }],
  // 连接中源和目标端点的默认外观
  EndpointStyle: { fill: '#1879ffa1', outlineWidth: 1 },
  // jsPlumb 的内部日志记录是否打开
  LogEnabled: true,
  // 连接器的默认外观
  PaintStyle: {
    stroke: '#E0E3E7',
    strokeWidth: 1,
    outlineStroke: 'transparent',
    outlineWidth: 10,
  },
  // 用于配置任何可拖动元素的默认选项jsPlumb.draggable
  DragOptions: { cursor: 'pointer', zIndex: 2000 },
  // 添加到连接器和端点的默认叠加层。已弃用：从 4.x 开始，将不支持此功能。并非所有叠加层都可以连接到连接器和端点�?
  Overlays: [
    [
      'Arrow',
      {
        width: 10, // 箭头尾部的宽�?
        length: 8, // 从箭头的尾部到头部的距离
        location: 1, // 位置，建议使�?�?之间
        direction: 1, // 方向，默认值为1（表示向前），可�?1（表示向后）
        foldback: 0.623, // 折回，也就是尾翼的角度，默认0.623，当�?时，为正三角
      },
    ],
    [
      'Label',
      {
        label: '',
        location: 0.5,
        cssClass: 'aLabel',
      },
    ],
  ],
  // 默认渲染模式 svg、canvas
  RenderMode: 'svg',
  // 悬停状态下连接的默认外�?
  HoverPaintStyle: { stroke: '#b0b2b5', strokeWidth: 1 },
  // 悬停状态下端点的默认外�?
  EndpointHoverStyle: { fill: 'red' },
  // 端点和连接的默认范围。范围提供了对哪些端点可以连接到哪些其他端点的基本控�?
  Scope: 'jsPlumb_DefaultScope',
}

// 整个节点作为source或者target
export const jsplumbMakeSource = {
  // 设置可以拖拽的类名，只要鼠标移动到该类名上的DOM，就可以拖拽连线
  filter: '.workflow-icon-drag',
  filterExclude: false,
  anchor: 'Continuous',
  // 是否允许自己连接自己
  allowLoopback: true,
  maxConnections: -1,
}

// 整个节点作为source或者target
export const jsplumbMakeTarget = {
  filter: '.workflow-icon-drag',
  filterExclude: false,
  // 是否允许自己连接自己
  anchor: 'Continuous',
  allowLoopback: true,
  dropOptions: { hoverClass: 'ef-drop-hover' },
}

// 连线参数
export const jsplumbConnect = {
  isSource: true,
  isTarget: true,
  // 动态锚点、提供了4个方�?Continuous、AutoDefault
  anchor: 'Continuous',
}
