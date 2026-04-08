// import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"

// export default function EnvironmentCard({ environment }) {
//   const { temperature, humidity, temp_warning, humidity_warning } = environment

//   return (
//     <Card className="w-full">
//       <CardHeader>
//         <CardTitle>🌡️ Environment</CardTitle>
//       </CardHeader>
//       <CardContent className="space-y-3">
//         <div className="flex items-center justify-between">
//           <span className="text-gray-600">Temperature</span>
//           <span className={`font-bold text-lg ${temp_warning ? "text-red-600" : "text-green-600"}`}>
//             {temperature}°C {temp_warning && "⚠️"}
//           </span>
//         </div>
//         <div className="flex items-center justify-between">
//           <span className="text-gray-600">Humidity</span>
//           <span className={`font-bold text-lg ${humidity_warning ? "text-red-600" : "text-green-600"}`}>
//             {humidity}% {humidity_warning && "⚠️"}
//           </span>
//         </div>
//         {(temp_warning || humidity_warning) && (
//           <p className="text-red-500 text-sm mt-2">⚠️ Environmental conditions out of safe range</p>
//         )}
//       </CardContent>
//     </Card>
//   )
// }

export default function EnvironmentCard({ environment }) {
  const { temperature, humidity, temp_warning, humidity_warning } = environment

  return (
    <div className="rounded-xl border border-purple-200 bg-white shadow-sm p-5">
      <h3 className="text-purple-700 font-semibold text-base mb-4">🌡️ Environment</h3>

      <div className="space-y-3">
        <div className="flex items-center justify-between bg-gray-50 rounded-lg px-4 py-3">
          <span className="text-gray-600 text-sm">Temperature</span>
          <span className={`font-bold text-lg ${temp_warning ? "text-red-600" : "text-green-600"}`}>
            {temperature}°C {temp_warning && "⚠️"}
          </span>
        </div>

        <div className="flex items-center justify-between bg-gray-50 rounded-lg px-4 py-3">
          <span className="text-gray-600 text-sm">Humidity</span>
          <span className={`font-bold text-lg ${humidity_warning ? "text-red-600" : "text-green-600"}`}>
            {humidity}% {humidity_warning && "⚠️"}
          </span>
        </div>

        {(temp_warning || humidity_warning) && (
          <div className="bg-red-50 border border-red-200 rounded-lg px-4 py-2 mt-2">
            <p className="text-red-500 text-sm">⚠️ Conditions out of safe range</p>
          </div>
        )}

        {(!temp_warning && !humidity_warning) && (
          <div className="bg-green-50 border border-green-200 rounded-lg px-4 py-2 mt-2">
            <p className="text-green-600 text-sm">✅ All conditions normal</p>
          </div>
        )}
      </div>
    </div>
  )
}