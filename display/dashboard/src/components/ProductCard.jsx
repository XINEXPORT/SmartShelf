// const statusConfig = {
//   ok:  { label: "In Stock", bg: "bg-green-500",  text: "text-green-700",  border: "border-green-200", light: "bg-green-50"  },
//   low: { label: "Low",      bg: "bg-yellow-400", text: "text-yellow-700", border: "border-yellow-200", light: "bg-yellow-50" },
//   out: { label: "Out",      bg: "bg-red-500",    text: "text-red-700",   border: "border-red-200",   light: "bg-red-50"   },
// }

// export default function ProductCard({ product }) {
//   const cfg = statusConfig[product.status]

//   return (
//     <div className={`flex items-center w-full rounded-xl border ${cfg.border} ${cfg.light} px-5 py-4 gap-5 shadow-sm`}>
      
//       {/* Left — name and warning */}
//       <div className="flex-1 min-w-0">
//         <p className="text-lg font-semibold text-gray-800">{product.name}</p>
//         {product.status === "low" && (
//           <p className="text-yellow-600 text-sm mt-1">⚠️ Running low — restock soon</p>
//         )}
//         {product.status === "out" && (
//           <p className="text-red-600 text-sm mt-1">🚨 Out of stock!</p>
//         )}
//         {product.status === "ok" && (
//           <p className="text-green-600 text-sm mt-1">✅ Sufficient stock</p>
//         )}
//       </div>

//       {/* Center — stock count badge */}
//       <div className={`flex flex-col items-center justify-center rounded-lg ${cfg.bg} text-white w-16 h-16 shadow-md flex-shrink-0`}>
//         <span className="text-2xl font-bold">{product.count}</span>
//         <span className="text-xs">{cfg.label}</span>
//       </div>

//       {/* Right — product image */}
//       <div className="w-16 h-16 rounded-lg overflow-hidden flex-shrink-0 shadow-sm border border-gray-200">
//         <img
//           src={product.image}
//           alt={product.name}
//           className="w-full h-full object-cover"
//           onError={(e) => { e.target.src = "https://placehold.co/80x80?text=?" }}
//         />
//       </div>

//     </div>
//   )
// }

const statusConfig = {
  ok:  { label: "In Stock", bg: "bg-green-500",  border: "border-slate-600", light: "bg-slate-700" },
  low: { label: "Low",      bg: "bg-yellow-400", border: "border-slate-600", light: "bg-slate-700" },
  out: { label: "Out",      bg: "bg-red-500",    border: "border-slate-600", light: "bg-slate-700" },
}

export default function ProductCard({ product }) {
  const cfg = statusConfig[product.status]

  return (
    <div className={`flex items-center w-full rounded-xl border ${cfg.border} ${cfg.light} px-5 py-4 gap-5 shadow-sm`}>

      {/* Left — name and warning */}
      <div className="flex-1 min-w-0">
        <p className="text-lg font-semibold text-white">{product.name}</p>
        {product.status === "low" && (
          <p className="text-yellow-400 text-sm mt-1">⚠️ Running low — restock soon</p>
        )}
        {product.status === "out" && (
          <p className="text-red-400 text-sm mt-1">🚨 Out of stock!</p>
        )}
        {product.status === "ok" && (
          <p className="text-green-400 text-sm mt-1">✅ Sufficient stock</p>
        )}
      </div>

      {/* Center — stock count badge */}
      <div className={`flex flex-col items-center justify-center rounded-lg ${cfg.bg} text-white w-16 h-16 shadow-md flex-shrink-0`}>
        <span className="text-2xl font-bold">{product.count}</span>
        <span className="text-xs">{cfg.label}</span>
      </div>

      {/* Right — product image */}
      <div className="w-16 h-16 rounded-lg overflow-hidden flex-shrink-0 shadow-sm border border-slate-600">
        <img
          src={product.image}
          alt={product.name}
          className="w-full h-full object-cover"
          onError={(e) => { e.target.src = "https://placehold.co/80x80?text=?" }}
        />
      </div>

    </div>
  )
}