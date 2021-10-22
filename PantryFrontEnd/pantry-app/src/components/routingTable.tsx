type RoutingTable = {
  [url: string]: number;
};

// Add more when needed
const pageToIndexLookup: RoutingTable = {
  "/inventory": 0,
  "/shoplist": 1,
  "/expiredbin": 2,
  "/recipes": 3,
  "/qrscan": 4,
  "/settings": 5,
  "/account": 6,
  // "/login": 7,
  // "/signup": 8,
};

export const pageToIndex: (link: string) => number = (link) => {
  const lookedUpIndex = pageToIndexLookup[link];

  if (lookedUpIndex) {
    return lookedUpIndex;
  }

  return 0;
};
