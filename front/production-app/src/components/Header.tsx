import { Link } from "react-router-dom";
import { MyRoutes } from "../Routes";
import useHeader from "../viewModel/useHeader";

// Style
import "./CSS/Header.css"

function Header() {
  const { state, changeState, sendState } = useHeader();
  return (
    <header>
      <div id="left">
        <button
          onClick={ async () => {
            const result = await sendState();
            if(result){ await changeState(); }
          }}
        >
          {/* { state ? <p>true</p> : <p>flase</p>} */}
          { state ? <p>Turn on</p> : <p>Turn of</p>}

        </button>
      </div>
      <div id="right">
        <Link to={MyRoutes.production}>
          <button>Production</button>
        </Link>
        <Link to={MyRoutes.info}>
          <button>Info/configuration</button>
        </Link>
      </div>
    </header>
  );
}

export default Header;