import './App.css'
import AppLayout, { ELayout } from './layout/app-layout';
export function App() {
  const AppContainer = AppLayout[ELayout.side];

  return <>
    <AppContainer></AppContainer>
  </>
}