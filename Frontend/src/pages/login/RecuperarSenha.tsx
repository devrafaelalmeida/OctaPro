import { Link, useNavigate } from "react-router-dom";
import { AuthLayout, AuthTitle, Field, inputClass, primaryButtonClass } from "@/components/AuthLayout";

export default function RecuperarSenhaPage() {
  const navigate = useNavigate();
  return (
    <AuthLayout>
      <AuthTitle>Recuperar Senha</AuthTitle>
      <p className="text-sm text-muted-foreground mb-5">
        Informe seu e-mail para receber o código de verificação.
      </p>
      <form
        onSubmit={(e) => {
          e.preventDefault();
          navigate("/verificar-codigo");
        }}
      >
        <Field label="E-mail">
          <input type="email" className={inputClass} placeholder="seu@email.com" required />
        </Field>
        <button type="submit" className={primaryButtonClass}>
          Recuperar
        </button>
      </form>
      <div className="mt-5 text-center">
        <Link to="/login" className="text-sm text-primary hover:text-primary-dark font-medium">
          Voltar ao login
        </Link>
      </div>
    </AuthLayout>
  );
}
