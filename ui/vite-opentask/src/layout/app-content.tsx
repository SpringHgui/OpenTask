import { Suspense } from 'react';
import { Outlet } from 'react-router-dom';
import AuthRouter from '../routes/auth-router';
import { Loader2 } from 'lucide-react';

export function AppContent() {
  return <Suspense
    fallback={<Loader2 className="m-auto h-6 w-6 animate-spin" />}
  >
    <AuthRouter>
      <Outlet></Outlet>
    </AuthRouter>
  </Suspense>
}
