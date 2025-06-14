import { createRouter, createWebHistory } from "vue-router";
import App from "./App.vue";
import store from "./store";

const routes = [
  {
    path: "/",
    name: "home",
    component: () => import("./pages/Home.vue")
  },
  {
    path: "/apps",
    name: "apps",
    component: App,
    meta: { requiresAuth: true },
    children: [
      {
        path: "",
        component: () => import("./components/AppManagement.vue"),
      },
      {
        path: ":appId/sessions/:sessionId",
        name: "session",
        component: () => import("./components/Session.vue"),
      },
    ],
  },
  {
    path: "/apps/:appId",
    name: "app",
    component: App,
    meta: { requiresAuth: true },
    children: [
      {
        path: "dashboard",
        name: "dashboard",
        component: () => import("./components/Dashboard.vue"),
      },
      {
        path: "sessions",
        name: "sessions",
        component: () => import("./components/Sessions.vue"),
      },
    ],
  },
  {
    path: "/login",
    name: "login",
    component: () => import("./pages/Auth.vue"),
  },
  {
    path: "/error",
    name: "error",
    component: () => import("./pages/Error.vue"),
  },
  {
    path: "/notfound",
    name: "notfound",
    component: () => import("./pages/NotFound.vue"),
  },
  {
    path: "/access",
    name: "access",
    component: () => import("./pages/Access.vue"),
  },
  {
    path: "/:pathMatch(.*)*",
    name: "NotFound",
    component: () => import("./pages/NotFound.vue"),
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to) => {
  if (to.meta.requiresAuth && !store.getters.isUserAuth) {
    return {
      path: "/login",
      // save the location we were at to come back later
      query: { redirect: to.fullPath },
    };
  }
});

export default router;
