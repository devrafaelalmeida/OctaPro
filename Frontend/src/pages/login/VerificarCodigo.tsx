import { Link, useNavigate } from "react-router-dom";
import { AuthLayout, AuthTitle, Field, inputClass, primaryButtonClass } from "@/components/AuthLayout";

export default function VerificarCodigoPage() {
  const navigate = useNavigate();
  return (
    <AuthLayout>
      <AuthTitle>Verificação de Identidade</AuthTitle>
      <div className="bg-background border-l-[3px] border-primary p-3 mb-5 text-sm text-foreground">
        Um código foi enviado ao e-mail informado. Informe este código no campo abaixo.
      </div>
      <form
        onSubmit={(e) => {
          e.preventDefault();
          navigate("/nova-senha");
        }}
      >
        <Field label="Código de Verificação">
          <input
            type="text"
            className={inputClass + " tracking-[0.5em] text-center font-semibold"}
            placeholder="______"
            maxLength={6}
            required
          />
        </Field>
        <button type="submit" className={primaryButtonClass}>
          Enviar
        </button>
      </form>
      <div className="mt-5 text-center">
        <Link to="/recuperar-senha" className="text-sm text-primary hover:text-primary-dark font-medium">
          Voltar
        </Link>
      </div>
    </AuthLayout>
  );
}
