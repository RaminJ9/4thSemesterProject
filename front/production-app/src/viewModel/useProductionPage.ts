import { useEffect, useState } from "react";

// Models
import type { machine } from "../model/machine";

function useProductionPage() {
    const [machines, setMachines] = useState<machine[]>([]);
    const [production, setProduction] = useState<string[][]>([]);

    const [message, setMessage] = useState<string>("");
    const showMessage = (text: string) => {
        setMessage(text);
        setTimeout(() => {
        setMessage("");
        }, 6000);
    };

    async function fetchMachines(): Promise<void> {
        const res = await fetch(`http://localhost:5253/api/machine`, {
            method: "GET"
        });
        console.log("Fetched:", res);
    
        setMachines(await res.json());
    }

    async function fetchProduction(): Promise<void> {
        const res = await fetch(`http://localhost:5253/api/production`, {
            method: "GET"
        });
        console.log("Fetched:", res);
        
        const data = await res.json();
        const guidOnly: string[][] = data.map((step: machine[]) =>
            step.map(machine => machine.guid)
        );

        setProduction(guidOnly);
    }

    async function saveProduction(productionLine: string[][]): Promise<boolean> {
        const res = await fetch("http://localhost:5253/api/production", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(productionLine)
        });
        console.log("Fetched:", res);
        
        const errorText = await res.text();
        if (!res.ok) {
            showMessage(errorText)
            return false;
        }
        return true
    }

    useEffect(() => {
        fetchMachines();
        fetchProduction();
    }, []);

    return { machines, production, saveProduction, message }
}

export default useProductionPage;