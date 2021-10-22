/* eslint-disable no-restricted-globals */
self.addEventListener("install", (event) => {
  console.log("Service worker installing…");

  // delete when in prod
  self.skipWaiting();

  event.waitUntil(
    caches
      .open("static-v1")
      .then((cache) =>
        cache.add(
          "https://fonts.googleapis.com/css2?family=Cambay:wght@400;700&display=swap"
        )
      )
  );
});

self.addEventListener("activate", (e) => {
  e.waitUntil(console.log("Activated!", e));
});

// self.addEventListener("fetch", (e) => {
//   console.log("Intercepted request", e);
//   const url = new URL(e.request.url);

//   if (
//     url.origin === location.origin &&
//     url.pathname ===
//       "https://fonts.googleapis.com/css2?family=Cambay:wght@400;700&display=swap"
//   ) {
//     e.respondWith(
//       caches.match(
//         "https://fonts.googleapis.com/css2?family=Cambay:wght@400;700&display=swap"
//       )
//     );
//   } else {
//     e.respondWith(fetch(e.request));
//   }
// });

self.addEventListener("push", (e) => {
  e.waitUntil(
    self.registration.showNotification("Test Push", {
      body: e.data.text(),
    })
  );
});