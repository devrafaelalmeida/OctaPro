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

const honorarios = [
  { num: "0001234-56.2024.8.26.0100", cliente: "Construtora Aliança Ltda.", valor: "R$ 12.500,00", venc: "15/06/2024", status: "pending" as const, label: "Pendente" },
  { num: "0007821-09.2024.8.26.0001", cliente: "Maria Eduarda Silva", valor: "R$ 3.800,00", venc: "10/05/2024", status: "paid" as const, label: "Pago" },
  { num: "0012345-78.2023.5.02.0042", cliente: "Indústrias Norte S.A.", valor: "R$ 28.000,00", venc: "01/04/2024", status: "overdue" as const, label: "Vencido" },
  { num: "0098765-43.2024.8.26.0100", cliente: "João Carlos Pereira", valor: "R$ 5.200,00", venc: "20/06/2024", status: "pending" as const, label: "Pendente" },
  { num: "0045612-78.2024.4.03.6100", cliente: "Tech Solutions ME", valor: "R$ 15.000,00", venc: "30/05/2024", status: "paid" as const, label: "Pago" },
  { num: "0033221-10.2023.8.26.0002", cliente: "Ana Beatriz Fernandes", valor: "R$ 7.450,00", venc: "12/03/2024", status: "overdue" as const, label: "Vencido" },
];

export default function HonorariosPage() {
  return (
    <>
      <PageTitle>Honorários</PageTitle>

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
          <label className={labelCls}>Status</label>
          <select className={inputCls}>
            <option>Todos</option>
            <option>Pendente</option>
            <option>Pago</option>
            <option>Vencido</option>
          </select>
        </div>
      </FiltersCard>

      <TableCard
        columns={["Número do Processo", "Cliente", "Valor", "Vencimento", "Status", "Ações"]}
        actionsRight={
          <PrimaryButton>
            <span className="inline-flex items-center gap-2">
              <Plus size={16} /> Novo Honorário
            </span>
          </PrimaryButton>
        }
      >
        {honorarios.map((h, i) => (
          <Row key={h.num + i} index={i}>
            <Cell className="font-medium">{h.num}</Cell>
            <Cell>{h.cliente}</Cell>
            <Cell className="font-semibold">{h.valor}</Cell>
            <Cell>{h.venc}</Cell>
            <Cell>
              <StatusBadge label={h.label} variant={h.status} />
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
