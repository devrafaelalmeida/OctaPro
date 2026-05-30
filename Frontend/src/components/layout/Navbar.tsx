export function Navbar() {
  return (
    <header className="h-16 bg-card border-b border-border px-6 flex items-center justify-end">
      <div className="flex items-center gap-3">
        <div className="text-right">
          <div className="text-sm font-medium text-foreground">Dr. Ricardo Alves</div>
          <div className="text-xs text-muted-foreground">ricardo@octapro.com</div>
        </div>
        <div className="avatar-round w-9 h-9 bg-primary text-primary-foreground flex items-center justify-center text-sm font-semibold">
          RA
        </div>
      </div>
    </header>
  );
}
