
const 西奥迪尼社会心理学 =
["2022051010","2022051011","2022051012","2022051013","2022051014","2022051015","2022051016","2022051017","2022051018","2022051019","202205102","2022051020","2022051021","2022051022","2022051023","2022051024","202205103","202205104","202205105","202205106","202205107","202205108","202205109"]

const 系统架构实现 =
["202205101"]
const cesiumjs =
["202204261", "2022042610", "2022042611", "2022042612", "2022042613", "2022042614", "2022042615", "2022042616", "2022042617", "2022042618", "2022042619", "202204262", "2022042620", "2022042621", "2022042622", "2022042623", "202204263", "202204264", "202204265", "202204266", "202204267", "202204268", "202204269"]

const BOOKMAGNETISM = 
["202204271"]


module.exports = {
  base: '/',
  title: 'woyaodangrapper',
  description: 'https://blog.taoistcore.com',
  head: [
    ['link', { rel: 'icon', href: '/logo.ico' }]
  ],
  themeConfig: {
    nav: [
      { text: '首页', link: '/' },
      { text: '计算机科学', items: [
        { text: '系统架构实现', link: '/ARCHITECTURE/computerscience/' },
        {
          text: '三维引擎',
          items: [
            { text: 'CesiumJS', link: '/ARCHITECTURE/cesiumjs/' },
          ]
        },
        

      ]},
      { text: '心理学', items: [
        { text: '社会心理学', items: [
          { text: '西奥迪尼社会心理学', link: '/ARCHITECTURE/SOCIALPSYCHOLOGYGOALSININTERACTION/' },
        ]},
      ] },
    
      { text: 'HappyBoy', link: '/ARCHITECTURE/BOOKMAGNETISM/' },
      { text: 'Github', link: 'https://github.com/light-come/blog' } //,

      /**
       {
          text: 'Languages',
          items: [
            { text: '中文', link: '/language/chinese' },
            { text: 'English', link: '/language/english' }
          ]
        }
      **/

    ],

    sidebar: {
      '/ARCHITECTURE/SOCIALPSYCHOLOGYGOALSININTERACTION/': [
        {
          title: '西奥迪尼社会心理学',
          collapsable: false,
          children: 西奥迪尼社会心理学
        }
      ],
      '/ARCHITECTURE/BOOKMAGNETISM/': [
        {
          title: '快乐小子',
          collapsable: false,
          children: BOOKMAGNETISM
        },
      ],
      '/ARCHITECTURE/computerscience/': [
        {
          title: '系统架构实现',
          collapsable: false,
          children: 系统架构实现
        },
      ],
      '/ARCHITECTURE/cesiumjs/': [
        {
          title: 'CesiumJS',
          collapsable: false,
          children: cesiumjs
        }
      ],
      // fallback
      '/': [
        ''
      ]
    },
    lastUpdated: '上次更新'
  },
  markdown: {
    lineNumbers: true
  },
  plugins: [    
    ['check-md', {
      pattern: 'ARCHITECTURE/BOOKMAGNETISM/*.md'
    }],
    ['@vuepress/back-to-top', true], ['seo', { /* options */ }],
    [
      'sitemap' , {
        hostname: 'https://blog.taoistcore.com/'
      }
    ]
    [
      '@vuepress/last-updated',
      {
        transformer: (timestamp, lang) => {
          // 不要忘了安装 moment
          const moment = require('moment')
          moment.locale('zh-cn')
          return moment(timestamp).fromNow();
        }
      }
    ]
  ],
};