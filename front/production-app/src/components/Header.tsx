import { Link } from "react-router-dom";
import { MyRoutes } from "../Routes";

function Header() {
  return (
    <header>
      <Link to={MyRoutes.production}>
        <button>Production</button>
      </Link>
      <Link to={MyRoutes.info}>
        <button>Info/configuration</button>
      </Link>
    </header>
  );
}

export default Header;