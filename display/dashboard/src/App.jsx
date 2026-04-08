// import ProductCard from "./components/ProductCard"
// import EnvironmentCard from "./components/EnvironmentCard"
// import AlertsPanel from "./components/AlertsPanel"
// import { mockProducts, mockEnvironment, mockAlerts } from "./mockData"

// export default function App() {
//   const totalProducts = mockProducts.length
//   const lowCount = mockProducts.filter(p => p.status === "low").length
//   const outCount  = mockProducts.filter(p => p.status === "out").length

//   return (
//     <div className="min-h-screen bg-gray-100 p-6">

//       {/* Header */}
//       <div className="mb-6 border-b-2 border-purple-300 pb-4">
//         <h1 className="text-3xl font-bold text-purple-800">📦 Smart Shelf Dashboard</h1>
//         <p className="text-gray-500 text-sm mt-1">Real-time inventory monitoring system</p>
//       </div>

//       {/* Stats Bar */}
//       <div className="flex gap-4 mb-6">
//         <div className="bg-white rounded-xl px-5 py-3 shadow-sm border border-gray-200 flex-1 text-center">
//           <p className="text-xs text-gray-400 uppercase tracking-wide">Total Products</p>
//           <p className="text-2xl font-bold text-purple-700">{totalProducts}</p>
//         </div>
//         <div className="bg-yellow-50 rounded-xl px-5 py-3 shadow-sm border border-yellow-200 flex-1 text-center">
//           <p className="text-xs text-gray-400 uppercase tracking-wide">Low Stock</p>
//           <p className="text-2xl font-bold text-yellow-600">{lowCount}</p>
//         </div>
//         <div className="bg-red-50 rounded-xl px-5 py-3 shadow-sm border border-red-200 flex-1 text-center">
//           <p className="text-xs text-gray-400 uppercase tracking-wide">Out of Stock</p>
//           <p className="text-2xl font-bold text-red-600">{outCount}</p>
//         </div>
//       </div>

//       {/* Main Layout — 2/3 + 1/3 */}
//       <div className="flex gap-6 items-start">

//         {/* Left — Inventory Section (2/3) */}
//         <div className="w-2/3 bg-white rounded-xl shadow-sm border border-purple-200 p-5">
//           <h2 className="text-purple-700 font-semibold text-lg mb-1">Inventory</h2>
//           <p className="text-gray-400 text-xs mb-4 border-b border-purple-100 pb-3">
//             Live stock levels per product
//           </p>
//           <div className="space-y-3">
//             {mockProducts.map(product => (
//               <ProductCard key={product.id} product={product} />
//             ))}
//           </div>
//         </div>

//         {/* Right — Status + Alerts (1/3) */}
//         <div className="w-1/3 space-y-4">
//           <div className="bg-white rounded-xl shadow-sm border border-purple-200 p-1">
//             <div className="border-b border-purple-100 px-4 pt-4 pb-3">
//               <h2 className="text-purple-700 font-semibold text-lg">Status</h2>
//               <p className="text-gray-400 text-xs">Environment & alerts</p>
//             </div>
//             <div className="p-4 space-y-4">
//               <EnvironmentCard environment={mockEnvironment} />
//               <AlertsPanel alerts={mockAlerts} />
//             </div>
//           </div>
//         </div>

//       </div>
//     </div>
//   )
// }

import ProductCard from "./components/ProductCard"
import EnvironmentCard from "./components/EnvironmentCard"
import AlertsPanel from "./components/AlertsPanel"
import { mockProducts, mockEnvironment, mockAlerts } from "./mockData"

export default function App() {
  const totalProducts = mockProducts.length
  const lowCount = mockProducts.filter(p => p.status === "low").length
  const outCount  = mockProducts.filter(p => p.status === "out").length

  return (
    <div className="min-h-screen bg-slate-900 p-6">

      {/* Header */}
      <div className="mb-6 border-b-2 border-slate-700 pb-4">
        <h1 className="text-3xl font-bold text-white">Smart Shelf Dashboard</h1>
        <p className="text-slate-400 text-sm mt-1">Real-time inventory monitoring system</p>
      </div>

      {/* Stats Bar */}
      <div className="flex gap-4 mb-6">
        <div className="bg-slate-800 rounded-xl px-5 py-3 shadow-sm border border-slate-700 flex-1 text-center">
          <p className="text-xs text-slate-400 uppercase tracking-wide">Total Products</p>
          <p className="text-2xl font-bold text-white">{totalProducts}</p>
        </div>
        <div className="bg-slate-800 rounded-xl px-5 py-3 shadow-sm border border-yellow-900 flex-1 text-center">
          <p className="text-xs text-slate-400 uppercase tracking-wide">Low Stock</p>
          <p className="text-2xl font-bold text-yellow-400">{lowCount}</p>
        </div>
        <div className="bg-slate-800 rounded-xl px-5 py-3 shadow-sm border border-red-900 flex-1 text-center">
          <p className="text-xs text-slate-400 uppercase tracking-wide">Out of Stock</p>
          <p className="text-2xl font-bold text-red-400">{outCount}</p>
        </div>
      </div>

      {/* Main Layout — 2/3 + 1/3 — same height */}
      <div className="flex gap-6 items-stretch h-[calc(100vh-220px)]">

        {/* Left — Inventory Section (2/3) */}
        <div className="w-2/3 bg-slate-800 rounded-xl shadow-sm border border-slate-700 p-5 flex flex-col overflow-hidden">
          <h2 className="text-white font-semibold text-lg mb-1">Inventory</h2>
          <p className="text-slate-400 text-xs mb-4 border-b border-slate-700 pb-3">
            Live stock levels per product
          </p>
          {/* Scrollable if many items */}
          <div className="space-y-3 overflow-y-auto flex-1 pr-1">
            {mockProducts.map(product => (
              <ProductCard key={product.id} product={product} />
            ))}
          </div>
        </div>

        {/* Right — Status + Alerts (1/3) */}
        <div className="w-1/3 bg-slate-800 rounded-xl shadow-sm border border-slate-700 p-5 flex flex-col overflow-hidden">
          <h2 className="text-white font-semibold text-lg mb-1">Status</h2>
          <p className="text-slate-400 text-xs mb-4 border-b border-slate-700 pb-3">
            Environment & alerts
          </p>
          <div className="space-y-4 overflow-y-auto flex-1 pr-1">
            <EnvironmentCard environment={mockEnvironment} />
            <AlertsPanel alerts={mockAlerts} />
          </div>
        </div>

      </div>
    </div>
  )
}