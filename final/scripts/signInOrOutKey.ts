import type { EventBusKey } from '@vueuse/core'

export const signInOrOutKey: EventBusKey<{ loggedIn: boolean }> = Symbol('symbol-key')