import { useState, useEffect } from "react"
import ProductCard from "./components/ProductCard"
import AlertsPanel from "./components/AlertsPanel"
import { mockDashboard } from "./mockData"

const API_URL = "http://localhost:31221/api/dashboard"
const USE_MOCK = false // flip to false when testing with Christine's backend

export default function App() {
  const [dashboard, setDashboard] = useState(mockDashboard)
  const [error, setError] = useState(null)

  useEffect(() => {
    if (USE_MOCK) return

    const fetchData = async () => {
      try {
        const res = await fetch(API_URL)
        if (!res.ok) throw new Error(`HTTP error: ${res.status}`)
        const data = await res.json()
        setDashboard(data)
        setError(null)
      } catch (err) {
        console.error("Failed to fetch:", err)
        setError("Could not reach API — showing last known data")
      }
    }

    fetchData()
    const interval = setInterval(fetchData, 5000)
    return () => clearInterval(interval)
  }, [])

  const { summary, inventory, alerts } = dashboard

  return (
    <div className="min-h-screen bg-slate-900 p-6">

      {/* Header */}
      <div className="mb-6 border-b-2 border-slate-700 pb-4 flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-white">Smart Shelf Dashboard</h1>
          <p className="text-slate-400 text-sm mt-1">Real-time inventory monitoring system</p>
        </div>
        {!USE_MOCK && (
          <div className="flex items-center gap-2">
            <div className={`w-2 h-2 rounded-full ${error ? "bg-red-500" : "bg-green-500 animate-pulse"}`} />
            <span className="text-slate-400 text-xs">{error ? "Connection lost" : "Live"}</span>
          </div>
        )}
      </div>

      {/* Error banner */}
      {error && (
        <div className="mb-4 bg-red-900 border border-red-700 rounded-lg px-4 py-3">
          <p className="text-red-300 text-sm">⚠️ {error}</p>
        </div>
      )}

      {/* Stats Bar */}
      <div className="flex gap-4 mb-6">
        <div className="bg-slate-800 rounded-xl px-5 py-3 shadow-sm border border-slate-700 flex-1 text-center">
          <p className="text-xs text-slate-400 uppercase tracking-wide">Total Products</p>
          <p className="text-2xl font-bold text-white">{summary.totalProducts}</p>
        </div>
        <div className="bg-slate-800 rounded-xl px-5 py-3 shadow-sm border border-yellow-900 flex-1 text-center">
          <p className="text-xs text-slate-400 uppercase tracking-wide">Low Stock</p>
          <p className="text-2xl font-bold text-yellow-400">{summary.lowStockProducts}</p>
        </div>
        <div className="bg-slate-800 rounded-xl px-5 py-3 shadow-sm border border-red-900 flex-1 text-center">
          <p className="text-xs text-slate-400 uppercase tracking-wide">Out of Stock</p>
          <p className="text-2xl font-bold text-red-400">{summary.outOfStockProducts}</p>
        </div>
      </div>

      {/* Main Layout */}
      <div className="flex gap-6 items-stretch h-[calc(100vh-220px)]">

        {/* Left — Inventory (2/3) */}
        <div className="w-2/3 bg-slate-800 rounded-xl shadow-sm border border-slate-700 p-5 flex flex-col overflow-hidden">
          <h2 className="text-white font-semibold text-lg mb-1">Inventory</h2>
          <p className="text-slate-400 text-xs mb-4 border-b border-slate-700 pb-3">
            Live stock levels per product
          </p>
          <div className="space-y-3 overflow-y-auto flex-1 pr-1">
            {inventory.length === 0 ? (
              <p className="text-slate-400 text-sm">No inventory data yet.</p>
            ) : (
              inventory.map(product => (
                <ProductCard key={product.productId} product={product} />
              ))
            )}
          </div>
        </div>

        {/* Right — Alerts (1/3) */}
        <div className="w-1/3 bg-slate-800 rounded-xl shadow-sm border border-slate-700 p-5 flex flex-col overflow-hidden">
          <h2 className="text-white font-semibold text-lg mb-1">Alerts</h2>
          <p className="text-slate-400 text-xs mb-4 border-b border-slate-700 pb-3">
            Low stock notifications
          </p>
          <div className="space-y-4 overflow-y-auto flex-1 pr-1">
            <AlertsPanel alerts={alerts} />
          </div>
        </div>

      </div>
    </div>
  )
}