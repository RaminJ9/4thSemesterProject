import { Routes, Route } from "react-router-dom";
import Production from "./pages/Production";
import Info from "./pages/Info";
import { MyRoutes } from "./Routes";

function App() {
  return (
    <>
    <Routes>
      <Route path={MyRoutes.production} element={<Production />} />
      <Route path={MyRoutes.info} element={<Info />} />
    </Routes>
    </>
  );
}

export default App;