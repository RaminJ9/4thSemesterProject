import { Routes, Route } from "react-router-dom";
import Production from "./pages/ProductionPage";
import Info from "./pages/InfoPage";
import { MyRoutes } from "./Routes";
import "./App.css"

function App() {
  return (
    <>
    <Routes>
      <Route path={MyRoutes.production} element={<Production />} />
      <Route path={MyRoutes.info} element={<Info />} />
      <Route path={MyRoutes.machineInfo} element={<Info />} />
    </Routes>
    </>
  );
}

export default App;