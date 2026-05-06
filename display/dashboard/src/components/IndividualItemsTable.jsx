export default function IndividualItemsTable({ items }) {
  return (
    <div className="mt-6 bg-slate-800 rounded-xl shadow-sm border border-slate-700 p-5 overflow-hidden">
      <div className="mb-4 border-b border-slate-700 pb-3">
        <h2 className="text-white font-semibold text-lg">Individual Items</h2>
      </div>

      <div className="overflow-x-auto">
        <table className="w-full table-fixed text-sm">
          <thead>
            <tr className="text-left text-xs uppercase text-slate-400 border-b border-slate-700">
              <th className="w-28 py-3 pr-4 font-medium">Product ID</th>
              <th className="py-3 pr-4 font-medium">EPC</th>
              <th className="w-24 py-3 pr-4 font-medium">RSSI</th>
              <th className="w-36 py-3 font-medium">Shelf</th>
            </tr>
          </thead>
          <tbody>
            {items.length === 0 ? (
              <tr>
                <td colSpan="4" className="py-5 text-center text-slate-400">
                  No individual items from the latest scan.
                </td>
              </tr>
            ) : (
              items.map(item => (
                <tr key={item.epc} className="border-b border-slate-700 last:border-b-0">
                  <td className="py-3 pr-4 text-slate-200">{item.productId}</td>
                  <td className="py-3 pr-4 font-mono text-xs text-slate-300 truncate" title={item.epc}>
                    {item.epc}
                  </td>
                  <td className="py-3 pr-4 text-slate-200">{item.rssi}</td>
                  <td className="py-3">
                    <span className="inline-flex rounded-md border border-slate-600 bg-slate-700 px-2 py-1 text-xs font-medium text-slate-200">
                      {item.shelf}
                    </span>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </div>
  )
}
