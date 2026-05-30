import { redirect } from "react-router-dom";
import { isAuthenticated } from "@/lib/auth";

/** Bloqueia rotas autenticadas quando não há token. Use em layouts/páginas protegidas. */
export function requireAuthLoader() {
  if (!isAuthenticated()) {
    throw redirect("/login");
  }
  return null;
}

/** Impede acesso às telas de login quando já autenticado. */
export function guestOnlyLoader() {
  if (isAuthenticated()) {
    throw redirect("/processos");
  }
  return null;
}

/** Redireciona a raiz conforme o estado de autenticação. */
export function rootRedirectLoader() {
  throw redirect(isAuthenticated() ? "/processos" : "/login");
}
