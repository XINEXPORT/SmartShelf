// export const mockProducts = [
//   { id: 1, name: "Coke",   count: 3, status: "ok"  },
//   { id: 2, name: "Pepsi",  count: 1, status: "low" },
//   { id: 3, name: "Water",  count: 0, status: "out" },
// ]

// export const mockEnvironment = {
//   temperature: 29,
//   humidity: 70,
//   temp_warning: true,
//   humidity_warning: false,
// }

// export const mockAlerts = [
//   { id: 1, product_name: "Water",  message: "Stock dropped to 0", timestamp: "2026-04-07 14:32" },
//   { id: 2, product_name: "Pepsi",  message: "Stock is low (1 remaining)", timestamp: "2026-04-07 15:10" },
// ]

export const mockProducts = [
  { id: 1, name: "Coke",   count: 3, status: "ok",  image: "https://images.unsplash.com/photo-1629203851122-3726ecdf080e?w=80&h=80&fit=crop" },
  { id: 2, name: "Pepsi",  count: 1, status: "low", image: "https://images.unsplash.com/photo-1553456558-aff63285bdd1?w=80&h=80&fit=crop" },
  { id: 3, name: "Water",  count: 0, status: "out", image: "https://images.unsplash.com/photo-1548839140-29a749e1cf4d?w=80&h=80&fit=crop" },
]

export const mockEnvironment = {
  temperature: 29,
  humidity: 70,
  temp_warning: true,
  humidity_warning: false,
}

export const mockAlerts = [
  { id: 1, product_name: "Water",  message: "Stock dropped to 0",       timestamp: "2026-04-07 14:32" },
  { id: 2, product_name: "Pepsi",  message: "Stock is low (1 remaining)", timestamp: "2026-04-07 15:10" },
]