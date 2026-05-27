import { useEffect, useState } from "react";
import Info from "../pages/InfoPage";

// Models
import type { machine } from "../model/machine";

function useInfoPage() {
    const [components, setComponents] = useState<string[]>([]);
    const [machines, setMachines] = useState<machine[]>([]);
    const [message, setMessage] = useState<string>("");
    
    const showMessage = (text: string) => {
        setMessage(text);
        setTimeout(() => {
        setMessage("");
        }, 6000);
    };

    async function fetchComponents(): Promise<void> {
        const res = await fetch(`http://localhost:5253/api/component`, {
            method: "GET"
        });
        console.log("Fetched:", res);
        
        const componentString: string = await res.text()
        const componentList: string[] = componentString.split(", ")
        
        setComponents(componentList);
    }

    async function fetchMachines(): Promise<void> {
        const res = await fetch(`http://localhost:5253/api/machine`, {
            method: "GET"
        });
        console.log("Fetched:", res);
    
        setMachines(await res.json());
    }

    async function addMachine(machine: machine): Promise<boolean> {
        const res = await fetch("http://localhost:5253/api/machine", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(machine)
        });
        console.log("Fetched:", res);
        await fetchMachines();
        
        const errorText = await res.text(); // it sends a stupid errormessage
        if (!res.ok) {
            showMessage(errorText);
            return false;
        }
        return true
    }

    async function removeMachine(guid: string): Promise<boolean> {
        const res = await fetch(`http://localhost:5253/api/machine/${guid}`, {
            method: "DELETE"
        });
        console.log("Fetched:", res);
        await fetchMachines();

        // const errorText = await res.text(); // also ugly
        if (!res.ok) {
            showMessage("Can't delete a machine that is used in production.")
            return false;
        }
        return true
    }

    useEffect(() => {
        fetchMachines();
        fetchComponents();
    }, [Info]);

    return { components, machines, addMachine, removeMachine, message }
}

export default useInfoPage;