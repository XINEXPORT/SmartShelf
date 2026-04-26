export const mockDashboard = {
  summary: {
    totalProducts: 3,
    lowStockProducts: 1,
    outOfStockProducts: 1,
    lastUpdated: "2026-04-19T14:32:00"
  },
  inventory: [
    { productId: 1, productName: "Coke",  count: 3, threshold: 2, isLowStock: false, isOutOfStock: false, statusText: "In stock",   imagePath: "https://images.unsplash.com/photo-1629203851122-3726ecdf080e?w=80&h=80&fit=crop" },
    { productId: 2, productName: "Pepsi", count: 1, threshold: 2, isLowStock: true,  isOutOfStock: false, statusText: "Low stock",  imagePath: "https://images.unsplash.com/photo-1553456558-aff63285bdd1?w=80&h=80&fit=crop" },
    { productId: 3, productName: "Water", count: 0, threshold: 2, isLowStock: false, isOutOfStock: true,  statusText: "Out of stock", imagePath: "https://images.unsplash.com/photo-1548839140-29a749e1cf4d?w=80&h=80&fit=crop" },
  ],
  alerts: [
    { id: 1, productName: "Water",  message: "Stock dropped to 0",        timestamp: "2026-04-19 14:32" },
    { id: 2, productName: "Pepsi",  message: "Stock is low (1 remaining)", timestamp: "2026-04-19 15:10" },
  ],
  status: {
    backendOnline: true,
    readerConnected: true,
    lastSuccessfulScan: "2026-04-19T14:30:00",
    sensorAvailable: false,
    sensorMessage: ""
  }
}