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

const processos = [
  { num: "0001234-56.2024.8.26.0100", cliente: "Construtora Aliança Ltda.", data: "12/03/2024", status: "active" as const, statusLabel: "Em andamento" },
  { num: "0007821-09.2024.8.26.0001", cliente: "Maria Eduarda Silva", data: "05/02/2024", status: "active" as const, statusLabel: "Em andamento" },
  { num: "0012345-78.2023.5.02.0042", cliente: "Indústrias Norte S.A.", data: "22/11/2023", status: "pending" as const, statusLabel: "Aguardando" },
  { num: "0098765-43.2024.8.26.0100", cliente: "João Carlos Pereira", data: "18/04/2024", status: "closed" as const, statusLabel: "Encerrado" },
  { num: "0045612-78.2024.4.03.6100", cliente: "Tech Solutions ME", data: "09/05/2024", status: "active" as const, statusLabel: "Em andamento" },
  { num: "0033221-10.2023.8.26.0002", cliente: "Ana Beatriz Fernandes", data: "30/10/2023", status: "closed" as const, statusLabel: "Encerrado" },
  { num: "0078912-34.2024.8.26.0100", cliente: "Restaurante Sabor & Arte", data: "14/06/2024", status: "active" as const, statusLabel: "Em andamento" },
  { num: "0066554-23.2024.5.02.0011", cliente: "Roberto Lima Souza", data: "21/01/2024", status: "pending" as const, statusLabel: "Aguardando" },
];

export default function ProcessosPage() {
  return (
    <>
      <PageTitle>Processos Judiciais</PageTitle>

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
          <label className={labelCls}>Data de Abertura</label>
          <input type="date" className={inputCls} />
        </div>
      </FiltersCard>

      <TableCard
        columns={["Número do Processo", "Cliente", "Data de Abertura", "Status", "Ações"]}
        actionsRight={
          <PrimaryButton>
            <span className="inline-flex items-center gap-2">
              <Plus size={16} /> Novo Processo
            </span>
          </PrimaryButton>
        }
      >
        {processos.map((p, i) => (
          <Row key={p.num} index={i}>
            <Cell className="font-medium">{p.num}</Cell>
            <Cell>{p.cliente}</Cell>
            <Cell>{p.data}</Cell>
            <Cell>
              <StatusBadge label={p.statusLabel} variant={p.status} />
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
