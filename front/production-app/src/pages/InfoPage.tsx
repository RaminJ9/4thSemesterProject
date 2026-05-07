import { MyRoutes } from "../Routes";
import { useState } from "react";
import { Link } from "react-router-dom";
import useInfoPage from "../viewModel/useInfoPage";
import { useParams, useNavigate } from "react-router-dom";

// models
import type { machine } from "../model/machine";

// Components
import Header from "../components/Header";

// Style
import "./CSS/Info.css"
function Info() {
  const navigate = useNavigate();

  const { guid } = useParams();
  const { components, machines, addMachine, removeMachine } = useInfoPage();

  const [makeMachine, setMakeMachine] = useState<machine>({
    name: "",
    connectionString: "",
    component: ""
  });
  const currentMaschine = machines.find(machine => machine.guid === guid);
  
  return (
    <>
      <Header />
      <main>
        <div id="nav">
          {machines.map( machine =>
            <Link key={machine.guid} to={MyRoutes.machineInfoPath(machine.guid)}>
              <button>{machine.name}</button>
            </Link>
          )}
          <Link to={MyRoutes.info}>
            <button>+</button>
          </Link>
        </div>

        <div id="info">
          <h1>Info Page</h1>
          {guid === undefined ?
            <div>
            
              <input
                type="text"
                value={makeMachine.name}
                onChange={(e) =>
                  setMakeMachine({
                    ...makeMachine,
                    name: e.target.value
                  })
                }
              />
              <input
                type="text"
                value={makeMachine.connectionString}
                onChange={(e) =>
                  setMakeMachine({
                    ...makeMachine,
                    connectionString: e.target.value
                  })
                }
              />

              <select
                value={makeMachine.component}
                onChange={(e) =>
                  setMakeMachine({
                    ...makeMachine,
                    component: e.target.value
                  })
                }
              >
                <option value="">Component</option>
                {components.map( component => 
                  <option value={component}>{component}</option>
                )}
              </select>

              <button
                className="action"
                onClick={async () => {
                  await addMachine(makeMachine);
                }}
              >
                Save
              </button>
            </div>
          :
            <div>
              <p>{currentMaschine?.name}</p>
              <p>{currentMaschine?.connectionString}</p>
              <p>{currentMaschine?.guid}</p>
              <button className="action" onClick={async () => {
                await removeMachine(guid);
                navigate(MyRoutes.info);
              }}> 
              Delete
              </button>
            </div>
          }
        </div>
      </main>
    </>
  );
}

export default Info;