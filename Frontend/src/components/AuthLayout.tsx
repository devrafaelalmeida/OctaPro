import { ReactNode } from "react";

export function AuthLayout({ children }: { children: ReactNode }) {
  return (
    <div className="min-h-screen w-full bg-background flex flex-col items-center justify-center px-4 py-10">
      <div className="mb-8 text-center">
        <h1 className="text-3xl font-bold tracking-tight text-primary">
          Octa<span className="text-primary-light">Pro</span>
        </h1>
        <p className="mt-1 text-xs uppercase tracking-[0.2em] text-muted-foreground">
          Gestão Jurídica
        </p>
      </div>
      <div
        className="w-full max-w-[420px] bg-card border border-border p-8"
        style={{ boxShadow: "0 1px 4px rgba(0,0,0,0.10)" }}
      >
        {children}
      </div>
    </div>
  );
}

export function AuthTitle({ children }: { children: ReactNode }) {
  return <h2 className="text-xl font-semibold text-foreground mb-6">{children}</h2>;
}

export function Field({
  label,
  children,
}: {
  label: string;
  children: ReactNode;
}) {
  return (
    <label className="block mb-4">
      <span className="block text-sm font-medium text-foreground mb-1.5">{label}</span>
      {children}
    </label>
  );
}

export const inputClass =
  "w-full h-10 px-3 border border-border bg-card text-foreground text-sm outline-none focus:border-primary focus:ring-1 focus:ring-primary transition-colors";

export const primaryButtonClass =
  "w-full h-10 bg-primary text-primary-foreground text-sm font-medium hover:bg-primary-dark transition-colors";
