import { useEffect, useState } from "react";

// Models
import type { machine } from "../model/machine";
import Info from "../pages/InfoPage";

function useInfoPage() {
    const [components, setComponents] = useState<string[]>([]);
    const [machines, setMachines] = useState<machine[]>([]);
    
    async function fetchComponents(): Promise<void> {
        const res = await fetch(`http://localhost:5253/api/component`, {
            method: "GET"
        });
        console.log("Fetched:", res);
        
        setComponents([await res.text()]);
    }

    async function fetchMachines(): Promise<void> {
        const res = await fetch(`http://localhost:5253/api/machine`, {
            method: "GET"
        });
        console.log("Fetched:", res);
    
        setMachines(await res.json());
    }

    async function addMachine(machine: machine): Promise<void> {
        const res = await fetch("http://localhost:5253/api/machine", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(machine)
        });
        console.log("Fetched:", res);

        await fetchMachines();
    }

    async function removeMachine(guid: string): Promise<void> {
        const res = await fetch(`http://localhost:5253/api/machine/${guid}`, {
            method: "DELETE"
        });
        console.log("Fetched:", res);

        await fetchMachines();
    }

    useEffect(() => {
        fetchMachines();
        fetchComponents();
    }, [Info]);

    return { components, machines, addMachine, removeMachine }
}

export default useInfoPage;