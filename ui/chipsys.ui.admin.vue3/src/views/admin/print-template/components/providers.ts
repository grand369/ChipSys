import { hiprint } from 'vue-plugin-hiprint'
import logoImg from '/@/assets/logo-mini.svg'

// 定义全局常量
export const COM_MODULE = 'comModule'

const comProvider = function () {
  var addElementTypes = function (context: any) {
    context.removePrintElementTypes(COM_MODULE)
    context.addPrintElementTypes(COM_MODULE, [
      new hiprint.PrintElementTypeGroup('常用组件', [
        {
          tid: `${COM_MODULE}.text`,
          title: '文本',
          data: '',
          type: 'text',
        },
        {
          tid: `${COM_MODULE}.longText`,
          title: '长文�?,
          data: '',
          type: 'longText',
          options: {
            width: 500,
          },
        },
        {
          tid: `${COM_MODULE}.table`,
          field: 'table',
          title: '表格',
          type: 'table',
          groupFields: ['name'],
          groupFooterFormatter: function (group: any, option: any) {
            return '这里自定义统计脚信息'
          },
          options: {
            width: 500,
          },
          columns: [
            [
              {
                title: '行号',
                fixed: true,
                rowspan: 2,
                field: 'id',
                width: 70,
              },
              { title: '人员信息', colspan: 2 },
              { title: '销售统�?, colspan: 2 },
            ],
            [
              {
                title: '姓名',
                align: 'left',
                field: 'name',
                width: 100,
              },
              { title: '性别', field: 'gender', width: 100 },
              {
                title: '销售数�?,
                field: 'count',
                width: 100,
              },
              {
                title: '销售金�?,
                field: 'amount',
                width: 100,
              },
            ],
          ],
          editable: true,
          columnDisplayEditable: true, //列显示是否能编辑
          columnDisplayIndexEditable: true, //列顺序显示是否能编辑
          columnTitleEditable: true, //列标题是否能编辑
          columnResizable: true, //列宽是否能调�?
          columnAlignEditable: true, //列对齐是否调�?
          isEnableEditField: true, //编辑字段
          isEnableContextMenu: true, //开启右键菜�?默认true
          isEnableInsertRow: true, //插入�?
          isEnableDeleteRow: true, //删除�?
          isEnableInsertColumn: true, //插入�?
          isEnableDeleteColumn: true, //删除�?
          isEnableMergeCell: true, //合并单元�?
        },
        {
          tid: `${COM_MODULE}.emptyTable`,
          title: '空白表格',
          type: 'table',
          options: {
            width: 500,
          },
          columns: [
            [
              {
                title: '',
                field: '',
                width: 100,
              },
              {
                title: '',
                field: '',
                width: 100,
              },
            ],
          ],
        },
        {
          tid: `${COM_MODULE}.html`,
          title: 'html',
          formatter: function (data: any, options: any) {
            return '<div style="height:50pt;width:50pt;background:red;border-radius: 50%;"></div>'
          },
          type: 'html',
        },
        {
          tid: `${COM_MODULE}.image`,
          title: '图片',
          type: 'image',
          options: { field: '', src: logoImg },
        },
      ]),
      new hiprint.PrintElementTypeGroup('辅助组件', [
        {
          tid: `${COM_MODULE}.hline`,
          title: '横线',
          type: 'hline',
        },
        {
          tid: `${COM_MODULE}.vline`,
          title: '竖线',
          type: 'vline',
        },
        {
          tid: `${COM_MODULE}.rect`,
          title: '矩形',
          type: 'rect',
        },
        {
          tid: `${COM_MODULE}.oval`,
          title: '椭圆',
          type: 'oval',
        },
        {
          tid: `${COM_MODULE}.barcode`,
          title: '条形�?,
          type: 'barcode',
        },
        {
          tid: `${COM_MODULE}.qrcode`,
          title: '二维�?,
          type: 'qrcode',
        },
      ]),
    ])
  }
  return {
    addElementTypes,
  }
}

export const dragElementGroups = [
  {
    name: 'common',
    title: '常用组件',
    elements: [
      {
        tid: `${COM_MODULE}.text`,
        title: '文本',
        icon: 'hiprint-text',
      },
      {
        tid: `${COM_MODULE}.longText`,
        title: '长文�?,
        icon: 'hiprint-longText',
      },
      {
        tid: `${COM_MODULE}.table`,
        title: '表格',
        icon: 'hiprint-table',
      },
      {
        tid: `${COM_MODULE}.emptyTable`,
        title: '空白表格',
        icon: 'hiprint-emptyTable',
      },
      {
        tid: `${COM_MODULE}.html`,
        title: 'html',
        icon: 'hiprint-html',
      },
      {
        tid: `${COM_MODULE}.image`,
        title: '图片',
        icon: 'hiprint-image',
      },
    ],
  },
  {
    name: 'help',
    title: '辅助组件',
    elements: [
      {
        tid: `${COM_MODULE}.hline`,
        title: '横线',
        icon: 'hiprint-hline',
      },
      {
        tid: `${COM_MODULE}.vline`,
        title: '竖线',
        icon: 'hiprint-vline',
      },
      {
        tid: `${COM_MODULE}.rect`,
        title: '矩形',
        icon: 'hiprint-rect',
      },
      {
        tid: `${COM_MODULE}.oval`,
        title: '椭圆',
        icon: 'hiprint-oval',
      },
      {
        tid: `${COM_MODULE}.barcode`,
        title: '条形�?,
        icon: 'hiprint-barcode',
      },
      {
        tid: `${COM_MODULE}.qrcode`,
        title: '二维�?,
        icon: 'hiprint-qrcode',
      },
    ],
  },
]

export default comProvider
