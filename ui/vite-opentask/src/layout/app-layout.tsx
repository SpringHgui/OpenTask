import React from 'react';
import Sidebar from './app-sidebar';
import { AppContent } from './app-content';
import Footer from './app-footer';
import Header from './app-header';

export const ELayout = {
  side: "side",
  fullPage: "fullPage",
}

export const SideLayout = React.memo(() => {
  return <>
    <div className="w-full h-full">
      <Header title={""}></Header >
      <div className='flex flex-row'>
        <Sidebar></Sidebar>
        <main className='flex flex-col w-full h-full relative p-10 overflow-auto' style={{ height: "calc(100vh - 3.5rem)" }}>
          <AppContent></AppContent>
          <Footer></Footer>
        </main>
      </div>
    </div>
  </>;
})

export const FullPageLayout = React.memo(() => <AppContent />);

export default {
  [ELayout.side]: SideLayout,
};


