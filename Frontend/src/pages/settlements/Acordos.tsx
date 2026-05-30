import { Plus } from "lucide-react";
import {
  PageTitle,
  FiltersCard,
  TableCard,
  Row,
  Cell,
  RowActions,
  StatusBadge,
  PrimaryButton,
  inputCls,
  labelCls,
} from "@/components/PagePrimitives";

const acordos = [
  { num: "0001234-56.2024.8.26.0100", cliente: "Construtora Aliança Ltda.", reclamante: "Carlos Henrique Dias", valor: "R$ 85.000,00", status: "negotiating" as const, label: "Em Negociação" },
  { num: "0007821-09.2024.8.26.0001", cliente: "Maria Eduarda Silva", reclamante: "Banco Itaú S.A.", valor: "R$ 22.450,00", status: "signed" as const, label: "Firmado" },
  { num: "0012345-78.2023.5.02.0042", cliente: "Indústrias Norte S.A.", reclamante: "Sindicato Metalúrgicos", valor: "R$ 145.000,00", status: "signed" as const, label: "Firmado" },
  { num: "0098765-43.2024.8.26.0100", cliente: "João Carlos Pereira", reclamante: "Imobiliária Central", valor: "R$ 38.000,00", status: "cancelled" as const, label: "Cancelado" },
  { num: "0045612-78.2024.4.03.6100", cliente: "Tech Solutions ME", reclamante: "Fornecedora ABC Ltda.", valor: "R$ 67.300,00", status: "negotiating" as const, label: "Em Negociação" },
  { num: "0033221-10.2023.8.26.0002", cliente: "Ana Beatriz Fernandes", reclamante: "Plano Saúde Vita", valor: "R$ 14.800,00", status: "signed" as const, label: "Firmado" },
];

export default function AcordosPage() {
  return (
    <>
      <PageTitle>Acordos</PageTitle>

      <FiltersCard>
        <div>
          <label className={labelCls}>Número do Processo</label>
          <input className={inputCls} placeholder="0000000-00.0000.0.00.0000" />
        </div>
        <div>
          <label className={labelCls}>Cliente</label>
          <input className={inputCls} placeholder="Nome do cliente" />
        </div>
        <div>
          <label className={labelCls}>Reclamante</label>
          <input className={inputCls} placeholder="Nome do reclamante" />
        </div>
        <div>
          <label className={labelCls}>Status</label>
          <select className={inputCls}>
            <option>Todos</option>
            <option>Em Negociação</option>
            <option>Firmado</option>
            <option>Cancelado</option>
          </select>
        </div>
      </FiltersCard>

      <TableCard
        columns={["Número do Processo", "Cliente", "Reclamante", "Valor do Acordo", "Status", "Ações"]}
        actionsRight={
          <PrimaryButton>
            <span className="inline-flex items-center gap-2">
              <Plus size={16} /> Novo Acordo
            </span>
          </PrimaryButton>
        }
      >
        {acordos.map((a, i) => (
          <Row key={a.num + i} index={i}>
            <Cell className="font-medium">{a.num}</Cell>
            <Cell>{a.cliente}</Cell>
            <Cell>{a.reclamante}</Cell>
            <Cell className="font-semibold">{a.valor}</Cell>
            <Cell>
              <StatusBadge label={a.label} variant={a.status} />
            </Cell>
            <Cell>
              <RowActions />
            </Cell>
          </Row>
        ))}
      </TableCard>
    </>
  );
}
