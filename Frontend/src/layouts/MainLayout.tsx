import { useState } from "react";
import { Outlet } from "react-router-dom";
import { Sidebar } from "@/components/layout/Sidebar";
import { Navbar } from "@/components/layout/Navbar";

export function MainLayout() {
  const [sidebarOpen, setSidebarOpen] = useState(true);

  return (
    <div className="min-h-screen w-full flex bg-background">
      <Sidebar open={sidebarOpen} onToggle={() => setSidebarOpen((s) => !s)} />
      <div className={`flex-1 flex flex-col min-h-screen transition-all duration-200`}>
        <Navbar />
        <main className="flex-1 p-6">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
