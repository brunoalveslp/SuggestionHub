/**
 * plugins/vuetify.ts
 *
 * Framework documentation: https://vuetifyjs.com`
 */

// Styles
import '@mdi/font/css/materialdesignicons.css'
import 'vuetify/styles'

// Composables
import { createVuetify } from 'vuetify'
import { VDataTable } from 'vuetify/components'

import colors, { purple } from 'vuetify/util/colors'

// https://vuetifyjs.com/en/introduction/why-vuetify/#feature-guides
export default createVuetify({
  components:{VDataTable},
  theme: {
    themes: {
      light: {
        dark: false,
        colors: {
          primary: colors.purple.darken4,
          secondary: colors.purple.lighten1
        }
      },
      dark: {
        dark: true,
        colors: {
          primary: colors.purple.lighten1,
          secondary: colors.purple.darken4
        }
      }
    }
  },
})
