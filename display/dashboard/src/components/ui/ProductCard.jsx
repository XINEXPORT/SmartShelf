// import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
// import { Badge } from "@/components/ui/badge"

// const statusConfig = {
//   ok:  { label: "In Stock", color: "bg-green-500" },
//   low: { label: "Low",      color: "bg-yellow-500" },
//   out: { label: "Out",      color: "bg-red-500" },
// }

// export default function ProductCard({ product }) {
//   const { label, color } = statusConfig[product.status]

//   return (
//     <Card className="w-full">
//       <CardHeader className="flex flex-row items-center justify-between pb-2">
//         <CardTitle className="text-lg">{product.name}</CardTitle>
//         <span className={`text-white text-xs px-2 py-1 rounded-full ${color}`}>
//           {label}
//         </span>
//       </CardHeader>
//       <CardContent>
//         <p className="text-4xl font-bold">{product.count}</p>
//         <p className="text-sm text-gray-500">units on shelf</p>
//         {product.status === "low" && (
//           <p className="text-yellow-600 text-sm mt-2">⚠️ Running low — restock soon</p>
//         )}
//         {product.status === "out" && (
//           <p className="text-red-600 text-sm mt-2">🚨 Out of stock!</p>
//         )}
//       </CardContent>
//     </Card>
//   )
// }

const statusConfig = {
  ok:  { label: "In Stock", bg: "bg-green-500",  text: "text-green-700",  border: "border-green-200", light: "bg-green-50"  },
  low: { label: "Low",      bg: "bg-yellow-400", text: "text-yellow-700", border: "border-yellow-200", light: "bg-yellow-50" },
  out: { label: "Out",      bg: "bg-red-500",    text: "text-red-700",   border: "border-red-200",   light: "bg-red-50"   },
}

export default function ProductCard({ product }) {
  const cfg = statusConfig[product.status]

  return (
    <div className={`flex items-center w-full rounded-xl border ${cfg.border} ${cfg.light} px-5 py-4 gap-5 shadow-sm`}>
      
      {/* Left — name and warning */}
      <div className="flex-1 min-w-0">
        <p className="text-lg font-semibold text-gray-800">{product.name}</p>
        {product.status === "low" && (
          <p className="text-yellow-600 text-sm mt-1">⚠️ Running low — restock soon</p>
        )}
        {product.status === "out" && (
          <p className="text-red-600 text-sm mt-1">🚨 Out of stock!</p>
        )}
        {product.status === "ok" && (
          <p className="text-green-600 text-sm mt-1">✅ Sufficient stock</p>
        )}
      </div>

      {/* Center — stock count badge */}
      <div className={`flex flex-col items-center justify-center rounded-lg ${cfg.bg} text-white w-16 h-16 shadow-md flex-shrink-0`}>
        <span className="text-2xl font-bold">{product.count}</span>
        <span className="text-xs">{cfg.label}</span>
      </div>

      {/* Right — product image */}
      <div className="w-16 h-16 rounded-lg overflow-hidden flex-shrink-0 shadow-sm border border-gray-200">
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