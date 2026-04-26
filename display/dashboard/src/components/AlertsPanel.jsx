export default function AlertsPanel({ alerts }) {
  return (
    <div className="rounded-xl border border-slate-600 bg-slate-700 p-5">
      <h3 className="text-slate-200 font-semibold text-base mb-4">🔔 Recent Alerts</h3>
      <div className="space-y-3">
        {alerts.length === 0 ? (
          <div className="bg-green-900 border border-green-700 rounded-lg px-4 py-3">
            <p className="text-green-300 text-sm">✅ No alerts — all good!</p>
          </div>
        ) : (
          alerts.map(alert => (
            <div key={alert.id} className="border-l-4 border-purple-500 bg-slate-600 rounded-r-lg pl-4 pr-3 py-3">
              <p className="font-semibold text-sm text-white">{alert.productName}</p>
              <p className="text-sm text-slate-300">{alert.message}</p>
              <p className="text-xs text-slate-400 mt-1">{alert.timestamp}</p>
            </div>
          ))
        )}
      </div>
    </div>
  )
}