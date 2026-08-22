class Base64 {
    static #textEncoder = new TextEncoder();
    static #textDecoder = new TextDecoder();

    // https://datatracker.ietf.org/doc/html/rfc4648#section-4
    static encode = (str) => btoa(String.fromCharCode(...Base64.#textEncoder.encode(str)));
    static decode = (str) => Base64.#textDecoder.decode(Uint8Array.from(atob(str), c => c.charCodeAt(0)));

    // https://datatracker.ietf.org/doc/html/rfc4648#section-5
    static encodeUrl = (str) => this.encode(str).replace(/\+/g, '-').replace(/\//g, '_'); //.replace(/=+$/, '');
    static decodeUrl = (str) => this.decode(str.replace(/\-/g, '+').replace(/\_/g, '/'));
}

document.addEventListener("submit", e => {
    const form = e.target;
    if (form.id === "auth-form") {
        // зупиняємо автоматичне відправлення форми
        e.preventDefault();
        const formData = new FormData(form);
        const login = formData.get("auth-login");
        const password = formData.get("auth-password");

        let errorMessage = "";
        if(login.trim().length == 0) {
            errorMessage += "Логін не може бути порожнім. ";
        }   
         if(password.trim().length == 0) {
            errorMessage += "Пароль не може бути порожнім. ";
        } 
         const err = document.getElementById("auth-modal-error");  
         if(errorMessage.length > 0) {
           err.innerText = errorMessage;
           err.style.visibility = "visible";
           return;
        }   
        else{
            err.innerText = "";
            err.style.visibility = "hidden";
        }
        // Передаємо дані до бекенду з дотриманням стандарту
        // https://datatracker.ietf.org/doc/html/rfc7617
        const userPass = login + ":" + password;
        const credentials = Base64.encode(userPass);
        fetch("/User/BasicAuth", {
            headers: {
                "Authorization": "Basic !" + credentials,
            }
        }).then(r => {
            if(r.ok) {
                return r.json();
            }
            else {
                return r.text();
            }
        }).then(console.log);
        console.log(credentials);
    }
});