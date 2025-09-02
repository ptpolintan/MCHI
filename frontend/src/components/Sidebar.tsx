"use client";

import { authService } from "@/lib/auth";
import { useRouter } from "next/navigation";

export default function Sidebar() {
  const router = useRouter();

  return (
    <div className="flex flex-col h-screen w-64 bg-gray-800 text-white">
      {/* Top section: App title */}
      <div className="p-6 text-xl font-bold border-b border-gray-700">
        MyApp
      </div>

      {/* Navigation links */}
      <nav className="flex-1 px-4 py-6 space-y-4">
        <button
          className="w-full text-left px-3 py-2 rounded hover:bg-gray-700"
          onClick={() => router.push("/checkins")}
        >
          Check-ins
        </button>
      </nav>

      {/* Bottom section: Logout */}
      <div className="p-4 border-t border-gray-700">
        <button
          className="w-full text-left px-3 py-2 rounded bg-red-600 hover:bg-red-700"
          onClick={() => {
            authService.clearToken();
            router.push("/login");
          }}
        >
          Logout
        </button>
      </div>
    </div>
  );
}