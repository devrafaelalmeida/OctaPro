import { createBrowserRouter, Navigate } from "react-router-dom";
import { MainLayout } from "@/layouts/MainLayout";
import LoginPage from "@/pages/login/Login";
import RecuperarSenhaPage from "@/pages/login/RecuperarSenha";
import VerificarCodigoPage from "@/pages/login/VerificarCodigo";
import NovaSenhaPage from "@/pages/login/NovaSenha";
import ProcessosPage from "@/pages/process/Processos";
import ClientesPage from "@/pages/clients/Clientes";
import HonorariosPage from "@/pages/legal-fees/Honorarios";
import AcordosPage from "@/pages/settlements/Acordos";
import { guestOnlyLoader, requireAuthLoader, rootRedirectLoader } from "./loaders";

export const router = createBrowserRouter([
  {
    path: "/",
    loader: rootRedirectLoader,
  },
  {
    path: "/login",
    element: <LoginPage />,
    loader: guestOnlyLoader,
  },
  {
    path: "/recuperar-senha",
    element: <RecuperarSenhaPage />,
    loader: guestOnlyLoader,
  },
  {
    path: "/verificar-codigo",
    element: <VerificarCodigoPage />,
    loader: guestOnlyLoader,
  },
  {
    path: "/nova-senha",
    element: <NovaSenhaPage />,
    loader: guestOnlyLoader,
  },
  {
    element: <MainLayout />,
    loader: requireAuthLoader,
    children: [
      { path: "/processos", element: <ProcessosPage /> },
      { path: "/clientes", element: <ClientesPage /> },
      { path: "/honorarios", element: <HonorariosPage /> },
      { path: "/acordos", element: <AcordosPage /> },
    ],
  },
  {
    path: "*",
    element: <Navigate to="/login" replace />,
  },
]);
