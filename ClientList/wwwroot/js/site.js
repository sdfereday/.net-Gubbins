class ClientModel {
  constructor(id = "", name = "", users = []) {
    this.name = name;
    this.id = id;
    this.users = users;
  }
}

class App {
  constructor() {
    this.clients = [];
    this.selectedClient = null;
    this.userNameEl = $("#username").find("#value");
    this.updatedUsernameEl = $("#updatedUsername");
    this.select = $("#clients").on("change", this.onClientSelected.bind(this));
    this.updateButton = $("#update").on(
      "click",
      this.onClientUpdated.bind(this)
    );
    return this;
  }

  getAllClients(cb) {
    fetch("http://localhost:8918/api/client")
      .then(function(response) {
        return response.json();
      })
      .then(function(res) {
        cb(res);
      });
  }

  getAllUsers(cb) {
    fetch("http://localhost:8918/api/user")
      .then(function(response) {
        return response.json();
      })
      .then(function(res) {
        cb(res);
      });
  }

  updateClient(data) {
    const { id } = data;
    return fetch("http://localhost:8918/api/client/" + id, {
      body: JSON.stringify(data), // must match 'Content-Type' header
      cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
      credentials: "same-origin", // include, same-origin, *omit
      headers: {
        "user-agent": "Mozilla/4.0 MDN Example",
        "content-type": "application/json"
      },
      method: "POST", // *GET, POST, PUT, DELETE, etc.
      mode: "cors", // no-cors, cors, *same-origin
      redirect: "follow", // manual, *follow, error
      referrer: "no-referrer" // *client, no-referrer
    });
  }

  onClientsReceived(res) {
    const { items } = res;
    if (items) {
      this.select.empty();
      this.clients = items.map(x => x);
    }
    this.clients.forEach(x => {
      const { id, name } = x;
      console.log(this.getAllUsers())
      // const clientModel = new ClientModel(id, name, this.getAllUsers().map(user => {
      //   console.log(user);
      //   return user;
      // }));

      // this.select.append(
      //   $("<option></option>")
      //     .attr("value", id)
      //     .text(name)
      // );

      //console.log(clientModel);

      //this.onClientSelected();
    }, this);
  }

  onClientSelected() {
    this.selectedClient = this.clients.find(
      ({ id }) => id === this.select.val(),
      10
    );

    if(!this.selectedClient) {
      return;
    }

    const { name } = this.selectedClient;
    this.userNameEl.text(name);
  }

  async onClientUpdated() {
    if (this.selectedClient) {
      await this.updateClient({
        ...this.selectedClient,
        name: this.updatedUsernameEl.val()
      });
      this.getAllClients(this.onClientsReceived.bind(this));
    }
  }

  start() {
    this.getAllClients(this.onClientsReceived.bind(this));
  }
}

new App().start();
