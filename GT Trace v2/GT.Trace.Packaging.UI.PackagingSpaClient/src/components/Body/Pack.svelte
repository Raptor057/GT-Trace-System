<script>
  export let packagingImageBase64Data = "";
  export let bomLabel = null;
  export let packType = "";
</script>

<div>
  <table border="0">
    <caption>
      <b><u>{packType}</u></b><br />
      <small>empaque</small>
    </caption>
    <tbody>
      <tr>
        <td>
          <img
            alt="Referencia"
            src={`data:image/png;base64,${packagingImageBase64Data}`}
          />
        </td>
      </tr>
      <tr>
        <td>
          <sub>Etiqueta individual</sub>
          <br />
          {bomLabel.number}
          {bomLabel.description.replace(`${bomLabel.number}:`, "")}
        </td>
      </tr>
    </tbody>
  </table>
</div>

<style>
  img {
    border: 1px blue solid;
    max-width: 100%;
  }
</style>
