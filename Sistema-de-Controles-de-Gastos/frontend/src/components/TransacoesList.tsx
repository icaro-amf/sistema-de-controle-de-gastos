import type { Transacao } from "../types";
import { formatarMoeda } from "../utils/format";

interface Props {
  transacoes: Transacao[];
}

function TransacoesList({ transacoes }: Props) {
  return (
    <div>
      <h2>Transacoes cadastradas</h2>
      {transacoes.length === 0 ? (
        <div className="empty-state">
          <div className="empty-state-icon">&#128179;</div>
          <p>Nenhuma transacao registrada ainda</p>
        </div>
      ) : (
        <ul className="transacao-list">
          {transacoes.map((t) => (
            <li key={t.id} className="transacao-item">
              <div style={{ display: "flex", alignItems: "center", gap: "10px" }}>
                <span className={`tipo-badge ${t.tipo.toLowerCase()}`}>{t.tipo}</span>
                <span className="transacao-descricao">{t.descricao}</span>
              </div>
              <span className={`transacao-valor ${t.tipo.toLowerCase()}`}>
                {t.tipo === "Receita" ? "+" : "-"} {formatarMoeda(t.valor)}
              </span>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}

export default TransacoesList;
