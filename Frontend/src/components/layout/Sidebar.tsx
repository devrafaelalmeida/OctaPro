import { Link, useLocation, useNavigate } from "react-router-dom";
import { clearAccessToken } from "@/lib/auth";
import {
  FolderKanban,
  Users,
  Wallet,
  Handshake,
  LogOut,
  ChevronLeft,
  ChevronRight,
} from "lucide-react";

const navItems = [
  { to: "/processos", label: "Processos Judiciais", icon: FolderKanban },
  { to: "/clientes", label: "Clientes", icon: Users },
  { to: "/honorarios", label: "Honorários", icon: Wallet },
  { to: "/acordos", label: "Acordos", icon: Handshake },
] as const;

type SidebarProps = {
  open: boolean;
  onToggle: () => void;
};

export function Sidebar({ open, onToggle }: SidebarProps) {
  const { pathname } = useLocation();
  const navigate = useNavigate();
  const width = open ? "w-[240px]" : "w-[60px]";

  return (
    <aside
      className={`${width} relative shrink-0 bg-sidebar text-sidebar-foreground flex flex-col fixed inset-y-0 left-0 transition-all duration-200`}
    >
      <div className="h-16 px-4 flex items-center border-b border-sidebar-border">
        <span
          className={`text-xl font-bold tracking-tight whitespace-nowrap overflow-hidden ${open ? "" : "opacity-0"}`}
        >
          Octa<span className="text-white/70">Pro</span>
        </span>
      </div>

      <nav className="flex-1 py-4">
        {navItems.map(({ to, label, icon: Icon }) => {
          const active = pathname.startsWith(to);
          return (
            <Link
              key={to}
              to={to}
              title={label}
              className={`flex items-center gap-3 px-4 h-11 text-sm border-l-[3px] transition-colors ${
                active
                  ? "bg-sidebar-primary border-white text-white"
                  : "border-transparent text-white/85 hover:bg-sidebar-accent hover:text-white"
              }`}
            >
              <Icon size={18} />
              <span
                className={`whitespace-nowrap overflow-hidden transition-opacity ${open ? "opacity-100" : "opacity-0 w-0"}`}
              >
                {label}
              </span>
            </Link>
          );
        })}
      </nav>

      <div className="border-t border-sidebar-border p-4 flex items-center justify-between">
        <div
          className={`text-sm whitespace-nowrap overflow-hidden transition-opacity ${open ? "opacity-100" : "opacity-0 w-0"}`}
        >
          <div className="font-medium">Dr. Ricardo Alves</div>
          <div className="text-xs text-white/60">Advogado</div>
        </div>
        <button
          onClick={() => {
            clearAccessToken();
            navigate("/login");
          }}
          className="text-white/80 hover:text-white"
          aria-label="Sair"
          title="Sair"
        >
          <LogOut size={18} />
        </button>
      </div>

      <button
        onClick={onToggle}
        className="rounded-full absolute top-1/2 right-0 translate-x-1/2 -translate-y-1/2 size-8 bg-card border border-border flex items-center justify-center text-foreground hover:bg-background transition-colors z-50"
        aria-label={open ? "Fechar menu" : "Abrir menu"}
        title={open ? "Fechar menu" : "Abrir menu"}
      >
        {open ? <ChevronLeft size={18} /> : <ChevronRight size={18} />}
      </button>
    </aside>
  );
}
