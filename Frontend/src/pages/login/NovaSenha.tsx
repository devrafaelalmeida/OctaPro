import { Link, useNavigate } from "react-router-dom";
import { useState } from "react";
import { Eye, EyeOff } from "lucide-react";
import { AuthLayout, AuthTitle, Field, inputClass, primaryButtonClass } from "@/components/AuthLayout";

export default function NovaSenhaPage() {
  const navigate = useNavigate();
  const [show1, setShow1] = useState(false);
  const [show2, setShow2] = useState(false);

  return (
    <AuthLayout>
      <AuthTitle>Definir Nova Senha</AuthTitle>
      <form
        onSubmit={(e) => {
          e.preventDefault();
          navigate("/login");
        }}
      >
        <Field label="Nova Senha">
          <div className="relative">
            <input type={show1 ? "text" : "password"} className={inputClass + " pr-10"} required />
            <button
              type="button"
              onClick={() => setShow1((v) => !v)}
              className="absolute right-2 top-1/2 -translate-y-1/2 text-muted-foreground hover:text-foreground"
            >
              {show1 ? <EyeOff size={18} /> : <Eye size={18} />}
            </button>
          </div>
        </Field>
        <Field label="Confirmar Nova Senha">
          <div className="relative">
            <input type={show2 ? "text" : "password"} className={inputClass + " pr-10"} required />
            <button
              type="button"
              onClick={() => setShow2((v) => !v)}
              className="absolute right-2 top-1/2 -translate-y-1/2 text-muted-foreground hover:text-foreground"
            >
              {show2 ? <EyeOff size={18} /> : <Eye size={18} />}
            </button>
          </div>
        </Field>
        <button type="submit" className={primaryButtonClass}>
          Salvar
        </button>
      </form>
      <div className="mt-5 text-center">
        <Link to="/login" className="text-sm text-primary hover:text-primary-dark font-medium">
          Voltar
        </Link>
      </div>
    </AuthLayout>
  );
}
