import { Link, useNavigate } from "react-router-dom";
import { useState } from "react";
import { Eye, EyeOff } from "lucide-react";
import { AuthLayout, AuthTitle, Field, inputClass, primaryButtonClass } from "@/components/AuthLayout";
import { setAccessToken } from "@/lib/auth";
import { authApi } from "@/services/auth";
import { useForm } from "react-hook-form";
import { loginSchema, type LoginSchema } from "@/schemas/auth.schema";
import { zodResolver } from "@hookform/resolvers/zod";

export default function LoginPage() {
  const navigate = useNavigate();
  const [showPwd, setShowPwd] = useState(false);

  const {
    register,
    handleSubmit,
    formState: { errors }
  } = useForm<LoginSchema>({resolver: zodResolver(loginSchema)});


  async function handleLogin(data: LoginSchema) {

    try {
      const payload = {
        email: data.email,
        password: data.password,
      }

      const response = await authApi.post("/auth/login", payload);

      const token = response.data.token;

      setAccessToken(token);

      navigate("/processos");

    } catch (error) {

      alert('Ocorreu um erro ao tentar fazer login!');

    }
  }

  return (
    <AuthLayout>
      <AuthTitle>Acesse sua conta</AuthTitle>
      <form onSubmit={handleSubmit(handleLogin)}>
        <Field label="E-mail">
          <input 
            type="email"
            className={inputClass}
            placeholder="email@email.com"
            {...register("email")}
             />
             {errors.email && (<span className="text-red-500 text-sm">{errors.email.message}</span>)}
        </Field>
        <Field label="Senha">
          <div className="relative">
            <input
              type={showPwd ? "text" : "password"}
              className={inputClass + " pr-10"}
              placeholder="••••••••"
              {...register("password")}
            />
            <button
              type="button"
              onClick={() => setShowPwd((v) => !v)}
              className="absolute right-2 top-1/2 -translate-y-1/2 text-muted-foreground hover:text-foreground"
              aria-label="Mostrar senha"
            >
              {showPwd ? <EyeOff size={18} /> : <Eye size={18} />}
            </button>
          </div>
          {errors.password && (<span className="text-red-500 text-sm">{errors.password.message}</span>)}
        </Field>
        <div className="flex justify-end mb-5">
          <Link to="/recuperar-senha" className="text-sm text-primary hover:text-primary-dark font-medium">
            Esqueceu sua senha?
          </Link>
        </div>
        <button type="submit" className={primaryButtonClass}>
          Entrar
        </button>
      </form>
    </AuthLayout>
  );
}
