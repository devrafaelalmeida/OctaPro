import { useState } from "react";
import { ChevronDown, User, Building2 } from "lucide-react";
import {
  PageTitle,
  FiltersCard,
  TableCard,
  Row,
  Cell,
  RowActions,
  inputCls,
  labelCls,
} from "@/components/PagePrimitives";

const clientes = [
  { nome: "Construtora Aliança Ltda.", tipo: "Jurídica", doc: "12.345.678/0001-90", email: "contato@alianca.com.br", tel: "(11) 3456-7890" },
  { nome: "Maria Eduarda Silva", tipo: "Física", doc: "123.456.789-00", email: "maria.silva@email.com", tel: "(11) 98765-4321" },
  { nome: "Indústrias Norte S.A.", tipo: "Jurídica", doc: "98.765.432/0001-10", email: "juridico@indnorte.com", tel: "(11) 2233-4455" },
  { nome: "João Carlos Pereira", tipo: "Física", doc: "987.654.321-00", email: "joao.pereira@email.com", tel: "(11) 91234-5678" },
  { nome: "Tech Solutions ME", tipo: "Jurídica", doc: "45.678.901/0001-23", email: "admin@techsolutions.com", tel: "(11) 4567-8901" },
  { nome: "Ana Beatriz Fernandes", tipo: "Física", doc: "456.789.123-45", email: "ana.fernandes@email.com", tel: "(11) 99887-7665" },
];

export default function ClientesPage() {
  const [openMenu, setOpenMenu] = useState(false);

  return (
    <>
      <PageTitle>Clientes</PageTitle>

      <FiltersCard>
        <div>
          <label className={labelCls}>Nome</label>
          <input className={inputCls} placeholder="Nome do cliente" />
        </div>
        <div>
          <label className={labelCls}>Documento (CPF/CNPJ)</label>
          <input className={inputCls} placeholder="000.000.000-00" />
        </div>
      </FiltersCard>

      <TableCard
        columns={["Nome", "Tipo", "Documento", "E-mail", "Telefone", "Ações"]}
        actionsRight={
          <div className="relative">
            <button
              onClick={() => setOpenMenu((v) => !v)}
              className="h-10 px-5 bg-primary text-primary-foreground text-sm font-medium hover:bg-primary-dark inline-flex items-center gap-2"
            >
              Novo <ChevronDown size={16} />
            </button>
            {openMenu && (
              <div
                className="absolute right-0 top-11 z-10 bg-card border border-border min-w-[200px]"
                style={{ boxShadow: "0 4px 12px rgba(0,0,0,0.15)" }}
              >
                <button
                  className="w-full px-4 py-3 text-left text-sm hover:bg-background flex items-center gap-2 text-foreground"
                  onClick={() => setOpenMenu(false)}
                >
                  <User size={16} className="text-primary" /> Pessoa Física
                </button>
                <button
                  className="w-full px-4 py-3 text-left text-sm hover:bg-background flex items-center gap-2 text-foreground border-t border-border"
                  onClick={() => setOpenMenu(false)}
                >
                  <Building2 size={16} className="text-primary" /> Pessoa Jurídica
                </button>
              </div>
            )}
          </div>
        }
      >
        {clientes.map((c, i) => (
          <Row key={c.doc} index={i}>
            <Cell className="font-medium">{c.nome}</Cell>
            <Cell>{c.tipo}</Cell>
            <Cell>{c.doc}</Cell>
            <Cell>{c.email}</Cell>
            <Cell>{c.tel}</Cell>
            <Cell>
              <RowActions />
            </Cell>
          </Row>
        ))}
      </TableCard>
    </>
  );
}
