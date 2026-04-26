// export default function EnvironmentCard({ environment }) {
//   const { temperature, humidity, temp_warning, humidity_warning } = environment

//   return (
//     <div className="rounded-xl border border-slate-600 bg-slate-700 p-5">
//       <h3 className="text-slate-200 font-semibold text-base mb-4">🌡️ Environment</h3>

//       <div className="space-y-3">
//         <div className="flex items-center justify-between bg-slate-600 rounded-lg px-4 py-3">
//           <span className="text-slate-300 text-sm">Temperature</span>
//           <span className={`font-bold text-lg ${temp_warning ? "text-red-400" : "text-green-400"}`}>
//             {temperature}°C {temp_warning && "⚠️"}
//           </span>
//         </div>

//         <div className="flex items-center justify-between bg-slate-600 rounded-lg px-4 py-3">
//           <span className="text-slate-300 text-sm">Humidity</span>
//           <span className={`font-bold text-lg ${humidity_warning ? "text-red-400" : "text-green-400"}`}>
//             {humidity}% {humidity_warning && "⚠️"}
//           </span>
//         </div>

//         {(temp_warning || humidity_warning) && (
//           <div className="bg-red-900 border border-red-700 rounded-lg px-4 py-2 mt-2">
//             <p className="text-red-300 text-sm">⚠️ Conditions out of safe range</p>
//           </div>
//         )}

//         {(!temp_warning && !humidity_warning) && (
//           <div className="bg-green-900 border border-green-700 rounded-lg px-4 py-2 mt-2">
//             <p className="text-green-300 text-sm">✅ All conditions normal</p>
//           </div>
//         )}
//       </div>
//     </div>
//   )
// }

// not needed anymore