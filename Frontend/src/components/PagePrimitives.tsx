import { ReactNode } from "react";

export function PageTitle({ children }: { children: ReactNode }) {
  return <h2 className="text-[22px] font-bold text-foreground mb-5">{children}</h2>;
}

export function Card({ children, className = "" }: { children: ReactNode; className?: string }) {
  return (
    <div
      className={`bg-card border border-border ${className}`}
      style={{ boxShadow: "0 1px 4px rgba(0,0,0,0.10)" }}
    >
      {children}
    </div>
  );
}

export const inputCls =
  "w-full h-10 px-3 border border-border bg-card text-sm text-foreground outline-none focus:border-primary focus:ring-1 focus:ring-primary";

export const labelCls = "block text-xs font-medium text-muted-foreground mb-1.5 uppercase tracking-wide";

export function PrimaryButton({
  children,
  onClick,
  type = "button",
  className = "",
}: {
  children: ReactNode;
  onClick?: () => void;
  type?: "button" | "submit";
  className?: string;
}) {
  return (
    <button
      type={type}
      onClick={onClick}
      className={`h-10 px-5 bg-primary text-primary-foreground text-sm font-medium hover:bg-primary-dark transition-colors ${className}`}
    >
      {children}
    </button>
  );
}

export function SecondaryButton({
  children,
  onClick,
  className = "",
}: {
  children: ReactNode;
  onClick?: () => void;
  className?: string;
}) {
  return (
    <button
      type="button"
      onClick={onClick}
      className={`h-10 px-5 bg-card text-primary text-sm font-medium border border-primary hover:bg-background transition-colors ${className}`}
    >
      {children}
    </button>
  );
}

export function FiltersCard({
  children,
  onSearch,
  onClear,
}: {
  children: ReactNode;
  onSearch?: () => void;
  onClear?: () => void;
}) {
  return (
    <Card className="p-5 mb-5">
      <h3 className="text-sm font-bold text-foreground mb-4 uppercase tracking-wide">Filtros</h3>
      <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mb-4">{children}</div>
      <div className="flex justify-end gap-2">
        <SecondaryButton onClick={onClear}>Limpar</SecondaryButton>
        <PrimaryButton onClick={onSearch}>Pesquisar</PrimaryButton>
      </div>
    </Card>
  );
}

export function TableCard({
  columns,
  children,
  actionsRight,
}: {
  columns: string[];
  children: ReactNode;
  actionsRight?: ReactNode;
}) {
  return (
    <>
      {actionsRight && <div className="flex justify-end mb-3 relative">{actionsRight}</div>}
      <Card>
        <div className="overflow-x-auto">
          <table className="w-full text-sm">
            <thead>
              <tr className="bg-primary text-primary-foreground">
                {columns.map((c) => (
                  <th
                    key={c}
                    className="text-left font-semibold px-4 py-3 uppercase tracking-wide text-xs"
                  >
                    {c}
                  </th>
                ))}
              </tr>
            </thead>
            <tbody>{children}</tbody>
          </table>
        </div>
        <div className="flex items-center justify-between px-4 py-3 border-t border-border text-xs text-muted-foreground">
          <span>Mostrando 1–10 de 24 registros</span>
          <div className="flex gap-1">
            <button className="h-8 px-3 border border-border bg-card hover:bg-background">Anterior</button>
            <button className="h-8 px-3 bg-primary text-primary-foreground">1</button>
            <button className="h-8 px-3 border border-border bg-card hover:bg-background">2</button>
            <button className="h-8 px-3 border border-border bg-card hover:bg-background">3</button>
            <button className="h-8 px-3 border border-border bg-card hover:bg-background">Próxima</button>
          </div>
        </div>
      </Card>
    </>
  );
}

export function Row({ children, index }: { children: ReactNode; index: number }) {
  return (
    <tr className={index % 2 === 0 ? "bg-card" : "bg-[color:var(--color-surface)]"}>
      {children}
    </tr>
  );
}

export function Cell({ children, className = "" }: { children: ReactNode; className?: string }) {
  return <td className={`px-4 py-3 border-b border-border text-foreground ${className}`}>{children}</td>;
}

export function RowActions() {
  return (
    <div className="flex gap-2">
      <button
        className="w-8 h-8 flex items-center justify-center text-primary hover:bg-background border border-border"
        aria-label="Editar"
        title="Editar"
      >
        <PencilIcon />
      </button>
      <button
        className="w-8 h-8 flex items-center justify-center text-destructive hover:bg-background border border-border"
        aria-label="Excluir"
        title="Excluir"
      >
        <TrashIcon />
      </button>
    </div>
  );
}

function PencilIcon() {
  return (
    <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
      <path d="M12 20h9" />
      <path d="M16.5 3.5a2.121 2.121 0 1 1 3 3L7 19l-4 1 1-4 12.5-12.5z" />
    </svg>
  );
}

function TrashIcon() {
  return (
    <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
      <polyline points="3 6 5 6 21 6" />
      <path d="M19 6l-1 14a2 2 0 0 1-2 2H8a2 2 0 0 1-2-2L5 6" />
      <path d="M10 11v6M14 11v6" />
      <path d="M9 6V4a2 2 0 0 1 2-2h2a2 2 0 0 1 2 2v2" />
    </svg>
  );
}

export function StatusBadge({
  label,
  variant,
}: {
  label: string;
  variant:
    | "pending"
    | "paid"
    | "overdue"
    | "negotiating"
    | "signed"
    | "cancelled"
    | "active"
    | "closed";
}) {
  const styles: Record<string, { bg: string; color: string }> = {
    pending: { bg: "#fff3cd", color: "#856404" },
    paid: { bg: "#d1e7dd", color: "#0a3622" },
    overdue: { bg: "#f8d7da", color: "#58151c" },
    negotiating: { bg: "#cfe2ff", color: "#084298" },
    signed: { bg: "#d1e7dd", color: "#0a3622" },
    cancelled: { bg: "#f8d7da", color: "#58151c" },
    active: { bg: "#cfe2ff", color: "#084298" },
    closed: { bg: "#e2e3e5", color: "#41464b" },
  };
  const s = styles[variant];
  return (
    <span
      className="inline-block px-2.5 py-1 text-xs font-semibold uppercase tracking-wide"
      style={{ backgroundColor: s.bg, color: s.color }}
    >
      {label}
    </span>
  );
}
