// const statusConfig = {
//   ok:  { label: "In Stock", bg: "bg-green-500",  border: "border-slate-600", light: "bg-slate-700" },
//   low: { label: "Low",      bg: "bg-yellow-400", border: "border-slate-600", light: "bg-slate-700" },
//   out: { label: "Out",      bg: "bg-red-500",    border: "border-slate-600", light: "bg-slate-700" },
// }

// export default function ProductCard({ product }) {
//   const cfg = statusConfig[product.status]

//   return (
//     <div className={`flex items-center w-full rounded-xl border ${cfg.border} ${cfg.light} px-5 py-4 gap-5 shadow-sm`}>

//       {/* Left — name and warning */}
//       <div className="flex-1 min-w-0">
//         <p className="text-lg font-semibold text-white">{product.name}</p>
//         {product.status === "low" && (
//           <p className="text-yellow-400 text-sm mt-1">⚠️ Running low — restock soon</p>
//         )}
//         {product.status === "out" && (
//           <p className="text-red-400 text-sm mt-1">🚨 Out of stock!</p>
//         )}
//         {product.status === "ok" && (
//           <p className="text-green-400 text-sm mt-1">✅ Sufficient stock</p>
//         )}
//       </div>

//       {/* Center — stock count badge */}
//       <div className={`flex flex-col items-center justify-center rounded-lg ${cfg.bg} text-white w-16 h-16 shadow-md flex-shrink-0`}>
//         <span className="text-2xl font-bold">{product.count}</span>
//         <span className="text-xs">{cfg.label}</span>
//       </div>

//       {/* Right — product image */}
//       <div className="w-16 h-16 rounded-lg overflow-hidden flex-shrink-0 shadow-sm border border-slate-600">
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

export default function ProductCard({ product }) {
  const getConfig = () => {
    if (product.isOutOfStock) return { label: "Out",      bg: "bg-red-500",    msg: "🚨 Out of stock!",             msgColor: "text-red-400"    }
    if (product.isLowStock)   return { label: "Low",      bg: "bg-yellow-400", msg: "⚠️ Running low — restock soon", msgColor: "text-yellow-400" }
    return                           { label: "In Stock", bg: "bg-green-500",  msg: "✅ Sufficient stock",            msgColor: "text-green-400"  }
  }

  const cfg = getConfig()

  return (
    <div className="flex items-center w-full rounded-xl border border-slate-600 bg-slate-700 px-5 py-4 gap-5 shadow-sm">

      {/* Left — name and status */}
      <div className="flex-1 min-w-0">
        <p className="text-lg font-semibold text-white">{product.productName}</p>
        <p className={`text-sm mt-1 ${cfg.msgColor}`}>{cfg.msg}</p>
        <p className="text-xs text-slate-400 mt-1">Threshold: {product.threshold}</p>
      </div>

      {/* Center — count badge */}
      <div className={`flex flex-col items-center justify-center rounded-lg ${cfg.bg} text-white w-16 h-16 shadow-md flex-shrink-0`}>
        <span className="text-2xl font-bold">{product.count}</span>
        <span className="text-xs">{cfg.label}</span>
      </div>

      {/* Right — image */}
      <div className="w-16 h-16 rounded-lg overflow-hidden flex-shrink-0 shadow-sm border border-slate-600">
        <img
          src={product.imagePath || "https://placehold.co/80x80?text=?"}
          alt={product.productName}
          className="w-full h-full object-cover"
          onError={(e) => { e.target.src = "https://placehold.co/80x80?text=?" }}
        />
      </div>

    </div>
  )
}