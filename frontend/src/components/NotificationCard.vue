<template>
          <v-card min-width="500px">
          <v-list>
            <v-list-item
              v-for="item in items"
              :key="item.id"
              :class="{ 'opacity-60': item.read }"
              class="transition-opacity"
              @click="markAsRead(item)"
            >
              <div class="d-flex justify-space-between align-center w-100">
                <div>
                  <div class="font-weight-medium">{{ item.title }}</div>
                  <div class="text-caption text--secondary">{{ item.subtitle }}</div>
                </div>

                <div class="d-flex flex-column align-end">
                  <small class="mb-4 text-high-emphasis opacity-60">{{ item.action }}</small>
                  <v-icon :color="item.read ? 'success' : 'warning'">
                    {{ item.read ? 'mdi-check-bold' : 'mdi-star-outline' }}
                  </v-icon>
                </div>
              </div>
            </v-list-item>
          </v-list>
        </v-card>
</template>
